using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Data;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Services;

public interface ICreateTestDataService
{
    Task<CreateTestDataResult> CreateBig();
    Task<CreateTestDataResult> CreateCourse();
}

public class CreateTestDataService : ICreateTestDataService
{
    private readonly StudyDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public CreateTestDataService(StudyDbContext dbContext, IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    public async Task<CreateTestDataResult> CreateBig()
    {
        var data = await LoadBigData();

        await ResetAcademicDemoData(data);

        return await SeedData(data);
    }

    public async Task<CreateTestDataResult> CreateCourse()
    {
        return await SeedData(CreateTestDataServiceData.CreateSingleCourseData());
    }

    private async Task<CreateTestDataSet> LoadBigData()
    {
        var filePath = Path.Combine(
            _environment.ContentRootPath,
            "Data",
            "Seed",
            "school-demo-data.json");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("School demo data file was not found.", filePath);

        var json = await File.ReadAllTextAsync(filePath);
        var data = JsonSerializer.Deserialize<CreateTestDataSet>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (data == null)
            throw new InvalidOperationException("School demo data file is empty or invalid.");

        return data;
    }

    private async Task ResetAcademicDemoData(CreateTestDataSet data)
    {
        var teacherEmails = data.Teachers
            .Select(t => t.Email)
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var demoCourseNames = data.Courses
            .Select(c => c.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var demoCourseIds = await _dbContext.Courses
            .Where(c =>
                demoCourseNames.Contains(c.Name) &&
                c.TeacherId.HasValue &&
                _dbContext.Users.Any(u =>
                    u.Id == c.TeacherId.Value &&
                    teacherEmails.Contains(u.Email)))
            .Select(c => c.Id)
            .ToListAsync();

        if (demoCourseIds.Count == 0)
            return;

        var demoAssignmentIds = await _dbContext.Assignments
            .Where(a => demoCourseIds.Contains(a.CourseId))
            .Select(a => a.Id)
            .ToListAsync();

        var demoAnnouncementIds = await _dbContext.Announcements
            .Where(a => demoCourseIds.Contains(a.CourseId))
            .Select(a => a.Id)
            .ToListAsync();

        var demoFolderIds = await _dbContext.StudyFolders
            .Where(f => demoCourseIds.Contains(f.CourseId))
            .Select(f => f.Id)
            .ToListAsync();

        var demoSubmissionIds = await _dbContext.Submissions
            .Where(s => demoAssignmentIds.Contains(s.AssignmentId))
            .Select(s => s.Id)
            .ToListAsync();

        var demoChatRoomIds = await _dbContext.ChatRooms
            .Where(cr =>
                cr.Type == ChatRoomType.Course &&
                cr.CourseId.HasValue &&
                demoCourseIds.Contains(cr.CourseId.Value))
            .Select(cr => cr.Id)
            .ToListAsync();

        _dbContext.ChatMessages.RemoveRange(
            await _dbContext.ChatMessages
                .Where(m => demoChatRoomIds.Contains(m.ChatRoomId))
                .ToListAsync());

        _dbContext.ChatRoomMembers.RemoveRange(
            await _dbContext.ChatRoomMembers
                .Where(m => demoChatRoomIds.Contains(m.ChatRoomId))
                .ToListAsync());

        _dbContext.ChatRooms.RemoveRange(
            await _dbContext.ChatRooms
                .Where(cr => demoChatRoomIds.Contains(cr.Id))
                .ToListAsync());

        _dbContext.StudyFiles.RemoveRange(
            await _dbContext.StudyFiles
                .Where(f =>
                    (f.AssignmentId.HasValue && demoAssignmentIds.Contains(f.AssignmentId.Value)) ||
                    (f.AnnouncementId.HasValue && demoAnnouncementIds.Contains(f.AnnouncementId.Value)) ||
                    (f.StudyFolderId.HasValue && demoFolderIds.Contains(f.StudyFolderId.Value)) ||
                    (f.SubmissionId.HasValue && demoSubmissionIds.Contains(f.SubmissionId.Value)))
                .ToListAsync());

        _dbContext.Submissions.RemoveRange(
            await _dbContext.Submissions
                .Where(s => demoSubmissionIds.Contains(s.Id))
                .ToListAsync());

        var demoFolders = await _dbContext.StudyFolders
            .Where(f => demoCourseIds.Contains(f.CourseId))
            .ToListAsync();

        _dbContext.StudyFolders.RemoveRange(OrderFoldersForDeletion(demoFolders));

        _dbContext.Assignments.RemoveRange(
            await _dbContext.Assignments
                .Where(a => demoCourseIds.Contains(a.CourseId))
                .ToListAsync());

        _dbContext.Announcements.RemoveRange(
            await _dbContext.Announcements
                .Where(a => demoCourseIds.Contains(a.CourseId))
                .ToListAsync());

        _dbContext.CourseStudents.RemoveRange(
            await _dbContext.CourseStudents
                .Where(cs => demoCourseIds.Contains(cs.CourseId))
                .ToListAsync());

        _dbContext.Courses.RemoveRange(
            await _dbContext.Courses
                .Where(c => demoCourseIds.Contains(c.Id))
                .ToListAsync());

        await _dbContext.SaveChangesAsync();
    }

    private async Task<CreateTestDataResult> SeedData(CreateTestDataSet data)
    {
        var result = new CreateTestDataResult();
        var userIdsByKey = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        await SeedUsers(data.Teachers, UserRole.Teacher, userIdsByKey, result);
        await SeedUsers(data.Students, UserRole.Student, userIdsByKey, result);

        await _dbContext.SaveChangesAsync();

        foreach (var courseSeed in data.Courses)
            await SeedCourse(courseSeed, userIdsByKey, result);

        await _dbContext.SaveChangesAsync();

        return result;
    }

    private async Task SeedUsers(
        List<CreateTestDataUserSeed> users,
        UserRole role,
        Dictionary<string, Guid> userIdsByKey,
        CreateTestDataResult result)
    {
        foreach (var userSeed in users)
        {
            if (string.IsNullOrWhiteSpace(userSeed.Key))
                throw new InvalidOperationException("User seed is missing a key.");

            if (string.IsNullOrWhiteSpace(userSeed.Email))
                throw new InvalidOperationException($"User seed '{userSeed.Key}' is missing an email.");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == userSeed.Email ||
                    (userSeed.Id.HasValue && u.Id == userSeed.Id.Value));

            if (user == null)
            {
                user = new User
                {
                    Id = userSeed.Id ?? Guid.NewGuid(),
                    CreatedAt = CreateTestDataServiceData.BaseDate
                };

                _dbContext.Users.Add(user);

                if (role == UserRole.Teacher)
                    result.TeachersCreated++;

                if (role == UserRole.Student)
                    result.StudentsCreated++;
            }
            else if (userSeed.Id.HasValue && user.Id != userSeed.Id.Value)
            {
                throw new InvalidOperationException(
                    $"User '{userSeed.Email}' already exists with id '{user.Id}', but the seed requires '{userSeed.Id}'.");
            }

            user.FirstName = userSeed.FirstName;
            user.LastName = userSeed.LastName;
            user.Email = userSeed.Email;
            user.PasswordHash = PasswordHelper.HashPassword(userSeed.Password);
            user.Role = role;

            userIdsByKey[userSeed.Key] = user.Id;
        }
    }

    private async Task SeedCourse(
        CreateTestDataCourseSeed courseSeed,
        Dictionary<string, Guid> userIdsByKey,
        CreateTestDataResult result)
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
                CreatedAt = CreateTestDataServiceData.BaseDate
            };

            _dbContext.Courses.Add(course);
            result.CoursesCreated++;
        }
        else
        {
            course.Description = courseSeed.Description;
        }

        var assignmentIdsByKey = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        var announcementIdsByKey = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        var folderIdsByKey = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
        var submissionIdsByKey = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        await SeedCourseStudents(course.Id, courseSeed.StudentKeys, userIdsByKey, result);
        await SeedAnnouncements(course.Id, courseSeed.Announcements, announcementIdsByKey, result);
        await SeedAssignments(course.Id, courseSeed.Assignments, userIdsByKey, assignmentIdsByKey, submissionIdsByKey,
            result);
        await SeedFolders(course.Id, courseSeed.Folders, folderIdsByKey, result);
        await SeedFiles(
            courseSeed.Files,
            userIdsByKey,
            assignmentIdsByKey,
            announcementIdsByKey,
            folderIdsByKey,
            submissionIdsByKey,
            result);

        await _dbContext.SaveChangesAsync();

        await SeedChat(course, courseSeed.ChatMessages, userIdsByKey, result);
    }

    private async Task SeedCourseStudents(
        Guid courseId,
        List<string> studentKeys,
        Dictionary<string, Guid> userIdsByKey,
        CreateTestDataResult result)
    {
        foreach (var studentKey in studentKeys.Distinct(StringComparer.OrdinalIgnoreCase))
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

            result.EnrollmentsCreated++;
        }
    }

    private async Task SeedAnnouncements(
        Guid courseId,
        List<CreateTestDataAnnouncementSeed> announcements,
        Dictionary<string, Guid> announcementIdsByKey,
        CreateTestDataResult result)
    {
        foreach (var announcementSeed in announcements)
        {
            var announcement = await _dbContext.Announcements
                .FirstOrDefaultAsync(a => a.CourseId == courseId && a.Name == announcementSeed.Name);

            var createdAt = ResolvePastCreatedAt(
                announcementSeed.CreatedAtOffsetDays,
                announcementSeed.CreatedAtOffsetMinutes,
                $"announcement '{announcementSeed.Key}'");

            if (announcement == null)
            {
                announcement = new Announcement
                {
                    Id = Guid.NewGuid(),
                    CourseId = courseId,
                    Name = announcementSeed.Name,
                    Content = announcementSeed.Content,
                    CreatedAt = createdAt
                };

                _dbContext.Announcements.Add(announcement);
                result.AnnouncementsCreated++;
            }
            else
            {
                announcement.Content = announcementSeed.Content;
                announcement.CreatedAt = createdAt;
            }

            announcementIdsByKey[announcementSeed.Key] = announcement.Id;
        }
    }

    private async Task SeedAssignments(
        Guid courseId,
        List<CreateTestDataAssignmentSeed> assignments,
        Dictionary<string, Guid> userIdsByKey,
        Dictionary<string, Guid> assignmentIdsByKey,
        Dictionary<string, Guid> submissionIdsByKey,
        CreateTestDataResult result)
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
                    CreatedAt = CreateTestDataServiceData.BaseDate
                };

                _dbContext.Assignments.Add(assignment);
                result.AssignmentsCreated++;
            }

            assignment.Description = assignmentSeed.Description;
            assignment.Deadline = assignmentSeed.DeadlineOffsetDays.HasValue
                ? CreateTestDataServiceData.BaseDate.Date.AddDays(assignmentSeed.DeadlineOffsetDays.Value)
                : null;

            assignmentIdsByKey[assignmentSeed.Key] = assignment.Id;

            await SeedSubmissions(assignment, assignmentSeed.Submissions, userIdsByKey, submissionIdsByKey, result);
        }
    }

    private async Task SeedSubmissions(
        Assignment assignment,
        List<CreateTestDataSubmissionSeed> submissions,
        Dictionary<string, Guid> userIdsByKey,
        Dictionary<string, Guid> submissionIdsByKey,
        CreateTestDataResult result)
    {
        foreach (var submissionSeed in submissions)
        {
            if (!userIdsByKey.TryGetValue(submissionSeed.StudentKey, out var studentId))
                throw new InvalidOperationException($"Student key '{submissionSeed.StudentKey}' was not found.");

            var submission = await _dbContext.Submissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignment.Id && s.StudentId == studentId);

            var submittedAt = CreateTestDataServiceData.BaseDate.Date.AddDays(submissionSeed.SubmittedAtOffsetDays);
            var grade = ParseNullableEnum<GradeLetter>(submissionSeed.Grade);
            var status = ParseSubmissionStatus(submissionSeed.Status, grade, submittedAt, assignment.Deadline);

            if (submission == null)
            {
                submission = new Submission
                {
                    Id = Guid.NewGuid(),
                    AssignmentId = assignment.Id,
                    StudentId = studentId,
                    CreatedAt = submittedAt
                };

                _dbContext.Submissions.Add(submission);
                result.SubmissionsCreated++;
            }

            submission.Comment = submissionSeed.Comment;
            submission.SubmittedAt = submittedAt;
            submission.Status = status;
            submission.Grade = grade;
            submission.Feedback = submissionSeed.Feedback;
            submission.GradedAt = grade.HasValue
                ? submittedAt.AddDays(2)
                : null;

            submissionIdsByKey[submissionSeed.Key] = submission.Id;
        }
    }

    private async Task SeedFolders(
        Guid courseId,
        List<CreateTestDataFolderSeed> folders,
        Dictionary<string, Guid> folderIdsByKey,
        CreateTestDataResult result)
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
                    CreatedAt = CreateTestDataServiceData.BaseDate
                };

                _dbContext.StudyFolders.Add(folder);
                result.StudyFoldersCreated++;
            }

            folderIdsByKey[folderSeed.Key] = folder.Id;
        }
    }

    private async Task SeedFiles(
        List<CreateTestDataFileSeed> files,
        Dictionary<string, Guid> userIdsByKey,
        Dictionary<string, Guid> assignmentIdsByKey,
        Dictionary<string, Guid> announcementIdsByKey,
        Dictionary<string, Guid> folderIdsByKey,
        Dictionary<string, Guid> submissionIdsByKey,
        CreateTestDataResult result)
    {
        foreach (var fileSeed in files)
        {
            ValidateFileSeed(fileSeed);

            if (!userIdsByKey.TryGetValue(fileSeed.UploadedByKey, out var uploadedById))
                throw new InvalidOperationException($"Uploader key '{fileSeed.UploadedByKey}' was not found.");

            var ownerIds = ResolveFileOwner(
                fileSeed,
                assignmentIdsByKey,
                announcementIdsByKey,
                folderIdsByKey,
                submissionIdsByKey);

            var existingFile = await FindExistingFile(fileSeed, ownerIds);

            if (existingFile != null)
                continue;

            _dbContext.StudyFiles.Add(new StudyFile
            {
                Id = Guid.NewGuid(),
                FileName = fileSeed.FileName,
                BlobName = fileSeed.SourceFileName,
                ContentType = fileSeed.ContentType,
                FileType = ParseRequiredEnum<FileType>(fileSeed.FileType, $"file '{fileSeed.Key}'"),
                Size = fileSeed.Size,
                AltText = fileSeed.AltText,
                UploadedById = uploadedById,
                StudyFolderId = ownerIds.FolderId,
                AssignmentId = ownerIds.AssignmentId,
                AnnouncementId = ownerIds.AnnouncementId,
                SubmissionId = ownerIds.SubmissionId,
                CreatedAt = CreateTestDataServiceData.BaseDate
            });

            result.StudyFilesCreated++;
        }
    }

    private static void ValidateFileSeed(CreateTestDataFileSeed fileSeed)
    {
        if (string.IsNullOrWhiteSpace(fileSeed.SourceFileName))
            throw new InvalidOperationException($"File seed '{fileSeed.Key}' is missing a source file name.");

        if (fileSeed.SourceFileName.StartsWith("studycentral-", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                $"File seed '{fileSeed.Key}' uses a studycentral-prefixed test file, which is not allowed.");

        if (!CreateTestDataServiceData.AllowedSourceFiles.Contains(fileSeed.SourceFileName))
            throw new InvalidOperationException(
                $"File seed '{fileSeed.Key}' uses unknown test data file '{fileSeed.SourceFileName}'.");
    }

    private static CreateTestDataResolvedOwner ResolveFileOwner(
        CreateTestDataFileSeed fileSeed,
        Dictionary<string, Guid> assignmentIdsByKey,
        Dictionary<string, Guid> announcementIdsByKey,
        Dictionary<string, Guid> folderIdsByKey,
        Dictionary<string, Guid> submissionIdsByKey)
    {
        var owner = new CreateTestDataResolvedOwner();

        if (fileSeed.OwnerType.Equals(CreateTestDataOwnerTypes.Folder, StringComparison.OrdinalIgnoreCase))
        {
            owner.FolderId = ResolveOwnerKey(fileSeed, folderIdsByKey);
            return owner;
        }

        if (fileSeed.OwnerType.Equals(CreateTestDataOwnerTypes.Assignment, StringComparison.OrdinalIgnoreCase))
        {
            owner.AssignmentId = ResolveOwnerKey(fileSeed, assignmentIdsByKey);
            return owner;
        }

        if (fileSeed.OwnerType.Equals(CreateTestDataOwnerTypes.Announcement, StringComparison.OrdinalIgnoreCase))
        {
            owner.AnnouncementId = ResolveOwnerKey(fileSeed, announcementIdsByKey);
            return owner;
        }

        if (fileSeed.OwnerType.Equals(CreateTestDataOwnerTypes.Submission, StringComparison.OrdinalIgnoreCase))
        {
            owner.SubmissionId = ResolveOwnerKey(fileSeed, submissionIdsByKey);
            return owner;
        }

        throw new InvalidOperationException(
            $"File seed '{fileSeed.Key}' has unknown owner type '{fileSeed.OwnerType}'.");
    }

    private static Guid ResolveOwnerKey(CreateTestDataFileSeed fileSeed, Dictionary<string, Guid> idsByKey)
    {
        if (!idsByKey.TryGetValue(fileSeed.OwnerKey, out var id))
            throw new InvalidOperationException(
                $"File seed '{fileSeed.Key}' references missing owner key '{fileSeed.OwnerKey}'.");

        return id;
    }

    private async Task<StudyFile?> FindExistingFile(
        CreateTestDataFileSeed fileSeed,
        CreateTestDataResolvedOwner ownerIds)
    {
        var query = _dbContext.StudyFiles
            .Where(f =>
                f.FileName == fileSeed.FileName &&
                f.BlobName == fileSeed.SourceFileName);

        if (ownerIds.FolderId.HasValue)
            return await query.FirstOrDefaultAsync(f => f.StudyFolderId == ownerIds.FolderId);

        if (ownerIds.AssignmentId.HasValue)
            return await query.FirstOrDefaultAsync(f => f.AssignmentId == ownerIds.AssignmentId);

        if (ownerIds.AnnouncementId.HasValue)
            return await query.FirstOrDefaultAsync(f => f.AnnouncementId == ownerIds.AnnouncementId);

        if (ownerIds.SubmissionId.HasValue)
            return await query.FirstOrDefaultAsync(f => f.SubmissionId == ownerIds.SubmissionId);

        return null;
    }

    private async Task SeedChat(
        Course course,
        List<CreateTestDataChatMessageSeed> messages,
        Dictionary<string, Guid> userIdsByKey,
        CreateTestDataResult result)
    {
        var chatRoom = await _dbContext.ChatRooms
            .FirstOrDefaultAsync(cr => cr.Type == ChatRoomType.Course && cr.CourseId == course.Id);

        if (chatRoom == null)
        {
            chatRoom = new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = $"{course.Name} Chat",
                Type = ChatRoomType.Course,
                CourseId = course.Id,
                CreatedAt = CreateTestDataServiceData.BaseDate
            };

            _dbContext.ChatRooms.Add(chatRoom);
            result.ChatRoomsCreated++;
        }

        if (course.TeacherId.HasValue)
            await EnsureChatRoomMember(chatRoom.Id, course.TeacherId.Value, result);

        var studentIds = await _dbContext.CourseStudents
            .Where(cs => cs.CourseId == course.Id)
            .Select(cs => cs.StudentId)
            .ToListAsync();

        foreach (var studentId in studentIds)
            await EnsureChatRoomMember(chatRoom.Id, studentId, result);

        foreach (var messageSeed in messages)
        {
            if (!userIdsByKey.TryGetValue(messageSeed.SenderKey, out var senderId))
                throw new InvalidOperationException($"Chat sender key '{messageSeed.SenderKey}' was not found.");

            if (course.TeacherId != senderId && !studentIds.Contains(senderId))
                throw new InvalidOperationException(
                    $"Chat sender key '{messageSeed.SenderKey}' is not a member of course '{course.Name}'.");

            var createdAt = CreateTestDataServiceData.BaseDate.AddMinutes(messageSeed.CreatedAtOffsetMinutes);
            var messageExists = await _dbContext.ChatMessages.AnyAsync(m =>
                m.ChatRoomId == chatRoom.Id &&
                m.SenderId == senderId &&
                m.Content == messageSeed.Content &&
                m.CreatedAt == createdAt);

            if (messageExists)
                continue;

            _dbContext.ChatMessages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoom.Id,
                SenderId = senderId,
                Content = messageSeed.Content,
                CreatedAt = createdAt
            });

            result.ChatMessagesCreated++;
        }
    }

    private async Task EnsureChatRoomMember(Guid chatRoomId, Guid userId, CreateTestDataResult result)
    {
        var exists = await _dbContext.ChatRoomMembers
            .AnyAsync(m => m.ChatRoomId == chatRoomId && m.UserId == userId);

        if (exists)
            return;

        _dbContext.ChatRoomMembers.Add(new ChatRoomMember
        {
            ChatRoomId = chatRoomId,
            UserId = userId,
            JoinedAt = CreateTestDataServiceData.BaseDate
        });

        result.ChatRoomMembersCreated++;
    }

    private static SubmissionStatus ParseSubmissionStatus(
        string? status,
        GradeLetter? grade,
        DateTime submittedAt,
        DateTime? deadline)
    {
        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<SubmissionStatus>(status, true, out var parsedStatus))
        {
            return parsedStatus;
        }

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

    private static TEnum ParseRequiredEnum<TEnum>(string value, string description) where TEnum : struct
    {
        if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse<TEnum>(value, true, out var parsedValue))
            return parsedValue;

        throw new InvalidOperationException($"Could not parse {typeof(TEnum).Name} for {description}.");
    }

    private static DateTime ResolvePastCreatedAt(int offsetDays, int offsetMinutes, string description)
    {
        var createdAt = CreateTestDataServiceData.BaseDate
            .Date
            .AddDays(offsetDays)
            .AddMinutes(offsetMinutes);

        if (createdAt >= DateTime.UtcNow)
            throw new InvalidOperationException(
                $"CreatedAt for {description} must be in the past. Offset days: {offsetDays}, offset minutes: {offsetMinutes}.");

        return createdAt;
    }

    private static List<StudyFolder> OrderFoldersForDeletion(List<StudyFolder> folders)
    {
        var parentIdsByFolderId = folders.ToDictionary(f => f.Id, f => f.ParentFolderId);

        return folders
            .OrderByDescending(f => GetFolderDepth(f.Id, parentIdsByFolderId))
            .ToList();
    }

    private static int GetFolderDepth(Guid folderId, Dictionary<Guid, Guid?> parentIdsByFolderId)
    {
        var depth = 1;
        var seen = new HashSet<Guid> { folderId };
        var currentId = folderId;

        while (parentIdsByFolderId.TryGetValue(currentId, out var parentId) && parentId.HasValue)
        {
            if (!seen.Add(parentId.Value))
                throw new InvalidOperationException("Cycle detected while clearing study folders.");

            depth++;
            currentId = parentId.Value;
        }

        return depth;
    }

    private sealed class CreateTestDataResolvedOwner
    {
        public Guid? FolderId { get; set; }
        public Guid? AssignmentId { get; set; }
        public Guid? AnnouncementId { get; set; }
        public Guid? SubmissionId { get; set; }
    }
}