using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Data.Seed;

public class SchoolDemoDataSeeder : ISchoolDemoDataSeeder
{
    private readonly StudyDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public SchoolDemoDataSeeder(StudyDbContext dbContext, IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    public async Task<SchoolDemoSeedResult> SeedAsync()
    {
        var filePath = Path.Combine(_environment.ContentRootPath, "Data", "Seed", "school-demo-data.json");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("School demo data file was not found.", filePath);

        var json = await File.ReadAllTextAsync(filePath);

        var demoData = JsonSerializer.Deserialize<SchoolDemoData>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (demoData == null)
            throw new InvalidOperationException("School demo data file is empty or invalid.");

        var result = new SchoolDemoSeedResult();

        var userIdsByKey = new Dictionary<string, Guid>();
        var courseIdsByKey = new Dictionary<string, Guid>();
        var assignmentIdsByKey = new Dictionary<string, Guid>();
        var folderIdsByKey = new Dictionary<string, Guid>();

        await SeedUsers(demoData.Teachers, UserRole.Teacher, userIdsByKey, result);
        await SeedUsers(demoData.Students, UserRole.Student, userIdsByKey, result);

        foreach (var courseSeed in demoData.Courses)
        {
            if (!userIdsByKey.TryGetValue(courseSeed.TeacherKey, out var teacherId))
                throw new InvalidOperationException($"Teacher key '{courseSeed.TeacherKey}' was not found.");

            var course = await _dbContext.Courses
                .FirstOrDefaultAsync(c => c.Name == courseSeed.Name && c.TeacherId == teacherId);

            if (course == null)
            {
                course = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = courseSeed.Name,
                    Description = courseSeed.Description,
                    TeacherId = teacherId,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Courses.Add(course);
                result.CoursesCreated++;
            }

            courseIdsByKey[courseSeed.Key] = course.Id;

            await SeedCourseStudents(course.Id, courseSeed.StudentKeys, userIdsByKey);
            await SeedAssignments(course.Id, courseSeed.Assignments, userIdsByKey, assignmentIdsByKey, result);
            await SeedAnnouncements(course.Id, courseSeed.Announcements, result);
            await SeedFolders(course.Id, courseSeed.Folders, folderIdsByKey, result);
        }

        await _dbContext.SaveChangesAsync();

        return result;
    }

    private async Task SeedUsers(List<UserSeed> users, UserRole role, Dictionary<string, Guid> userIdsByKey,
        SchoolDemoSeedResult result)
    {
        foreach (var userSeed in users)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userSeed.Email);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = userSeed.FirstName,
                    LastName = userSeed.LastName,
                    Email = userSeed.Email,
                    PasswordHash = PasswordHelper.HashPassword(userSeed.Password),
                    Role = role,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Users.Add(user);

                if (role == UserRole.Teacher)
                    result.TeachersCreated++;

                if (role == UserRole.Student)
                    result.StudentsCreated++;
            }

            userIdsByKey[userSeed.Key] = user.Id;
        }
    }

    private async Task SeedCourseStudents(Guid courseId, List<string> studentKeys,
        Dictionary<string, Guid> userIdsByKey)
    {
        foreach (var studentKey in studentKeys)
        {
            if (!userIdsByKey.TryGetValue(studentKey, out var studentId))
                throw new InvalidOperationException($"Student key '{studentKey}' was not found.");

            var enrollmentExists = await _dbContext.CourseStudents
                .AnyAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

            if (enrollmentExists)
                continue;

            _dbContext.CourseStudents.Add(new CourseStudent
            {
                CourseId = courseId,
                StudentId = studentId
            });
        }
    }

    private async Task SeedAssignments(Guid courseId, List<AssignmentSeed> assignments,
        Dictionary<string, Guid> userIdsByKey, Dictionary<string, Guid> assignmentIdsByKey, SchoolDemoSeedResult result)
    {
        foreach (var assignmentSeed in assignments)
        {
            var assignment = await _dbContext.Assignments
                .FirstOrDefaultAsync(a => a.CourseId == courseId && a.Name == assignmentSeed.Name);

            if (assignment == null)
            {
                assignment = new Assignment
                {
                    Id = Guid.NewGuid(),
                    CourseId = courseId,
                    Name = assignmentSeed.Name,
                    Description = assignmentSeed.Description,
                    Deadline = assignmentSeed.DeadlineOffsetDays.HasValue
                        ? DateTime.UtcNow.Date.AddDays(assignmentSeed.DeadlineOffsetDays.Value)
                        : null,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Assignments.Add(assignment);
                result.AssignmentsCreated++;
            }

            assignmentIdsByKey[assignmentSeed.Key] = assignment.Id;

            await SeedSubmissions(assignment, assignmentSeed.Submissions, userIdsByKey, result);
        }
    }

    private async Task SeedSubmissions(Assignment assignment, List<SubmissionSeed> submissions,
        Dictionary<string, Guid> userIdsByKey, SchoolDemoSeedResult result)
    {
        foreach (var submissionSeed in submissions)
        {
            if (!userIdsByKey.TryGetValue(submissionSeed.StudentKey, out var studentId))
                throw new InvalidOperationException($"Student key '{submissionSeed.StudentKey}' was not found.");

            var submissionExists = await _dbContext.Submissions
                .AnyAsync(s => s.AssignmentId == assignment.Id && s.StudentId == studentId);

            if (submissionExists)
                continue;

            var submittedAt = DateTime.UtcNow.Date.AddDays(submissionSeed.SubmittedAtOffsetDays);
            var grade = ParseNullableEnum<GradeLetter>(submissionSeed.Grade);
            var status = ParseSubmissionStatus(submissionSeed.Status, grade, submittedAt, assignment.Deadline);

            var submission = new Submission
            {
                Id = Guid.NewGuid(),
                AssignmentId = assignment.Id,
                StudentId = studentId,
                Comment = submissionSeed.Comment,
                SubmittedAt = submittedAt,
                Status = status,
                Grade = grade,
                Feedback = submissionSeed.Feedback,
                GradedAt = grade.HasValue ? DateTime.UtcNow : null,
                CreatedAt = submittedAt
            };

            _dbContext.Submissions.Add(submission);
            result.SubmissionsCreated++;
        }
    }

    private async Task SeedAnnouncements(Guid courseId, List<AnnouncementSeed> announcements,
        SchoolDemoSeedResult result)
    {
        foreach (var announcementSeed in announcements)
        {
            var announcementExists = await _dbContext.Announcements
                .AnyAsync(a => a.CourseId == courseId && a.Name == announcementSeed.Name);

            if (announcementExists)
                continue;

            _dbContext.Announcements.Add(new Announcement
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                Name = announcementSeed.Name,
                Content = announcementSeed.Content,
                CreatedAt = DateTime.UtcNow
            });

            result.AnnouncementsCreated++;
        }
    }

    private async Task SeedFolders(Guid courseId, List<FolderSeed> folders, Dictionary<string, Guid> folderIdsByKey,
        SchoolDemoSeedResult result)
    {
        foreach (var folderSeed in folders)
        {
            Guid? parentFolderId = null;

            if (!string.IsNullOrWhiteSpace(folderSeed.ParentFolderKey))
            {
                if (!folderIdsByKey.TryGetValue(folderSeed.ParentFolderKey, out var resolvedParentFolderId))
                    throw new InvalidOperationException(
                        $"Parent folder key '{folderSeed.ParentFolderKey}' was not found.");

                parentFolderId = resolvedParentFolderId;
            }

            var folder = await _dbContext.StudyFolders
                .FirstOrDefaultAsync(f =>
                    f.CourseId == courseId &&
                    f.Name == folderSeed.Name &&
                    f.ParentFolderId == parentFolderId);

            if (folder == null)
            {
                folder = new StudyFolder
                {
                    Id = Guid.NewGuid(),
                    CourseId = courseId,
                    Name = folderSeed.Name,
                    ParentFolderId = parentFolderId,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.StudyFolders.Add(folder);
                result.StudyFoldersCreated++;
            }

            folderIdsByKey[folderSeed.Key] = folder.Id;
        }
    }

    private static SubmissionStatus ParseSubmissionStatus(string? status, GradeLetter? grade, DateTime submittedAt,
        DateTime? deadline)
    {
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<SubmissionStatus>(status, true, out var parsedStatus))
            return parsedStatus;

        if (grade == GradeLetter.F)
            return SubmissionStatus.Failed;

        if (grade.HasValue)
            return SubmissionStatus.Passed;

        if (deadline.HasValue && submittedAt > deadline.Value)
            return SubmissionStatus.SubmittedLate;

        return SubmissionStatus.Submitted;
    }

    private static TEnum? ParseNullableEnum<TEnum>(string? value) where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return Enum.TryParse<TEnum>(value, true, out var parsedValue)
            ? parsedValue
            : null;
    }
}

public class SchoolDemoData
{
    public List<UserSeed> Teachers { get; set; } = new();

    public List<UserSeed> Students { get; set; } = new();

    public List<CourseSeed> Courses { get; set; } = new();
}

public class UserSeed
{
    public string Key { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = "Password123!";
}

public class CourseSeed
{
    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string TeacherKey { get; set; } = string.Empty;

    public List<string> StudentKeys { get; set; } = new();

    public List<AssignmentSeed> Assignments { get; set; } = new();

    public List<AnnouncementSeed> Announcements { get; set; } = new();

    public List<FolderSeed> Folders { get; set; } = new();
}

public class AssignmentSeed
{
    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int? DeadlineOffsetDays { get; set; }

    public List<SubmissionSeed> Submissions { get; set; } = new();
}

public class SubmissionSeed
{
    public string StudentKey { get; set; } = string.Empty;

    public string? Comment { get; set; }

    public int SubmittedAtOffsetDays { get; set; }

    public string? Status { get; set; }

    public string? Grade { get; set; }

    public string? Feedback { get; set; }
}

public class AnnouncementSeed
{
    public string Name { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}

public class FolderSeed
{
    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? ParentFolderKey { get; set; }
}