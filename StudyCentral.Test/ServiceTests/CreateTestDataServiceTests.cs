using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudyCentral.API.Data;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Services;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class CreateTestDataServiceTests
{
    [Fact]
    public async Task CreateBig_WhenRunFromJson_CreatesExpectedDemoSchool()
    {
        // Arrange
        await using var dbContext = ContextGenerator.GetStudyDbContext();
        await SeedExistingLoginUsers(dbContext);

        var service = new CreateTestDataService(dbContext, CreateEnvironment());

        // Act
        await service.CreateBig();
        await service.CreateBig();

        // Assert
        Assert.Equal(2, await dbContext.Users
            .CountAsync(u =>
                u.Role == UserRole.Teacher &&
                (u.Email == "teacher@studycentral.dk" ||
                 u.Email == "maria.lund.teacher@studycentral.dk")));

        Assert.Equal(50, await dbContext.Users
            .CountAsync(u =>
                u.Role == UserRole.Student &&
                u.Email.EndsWith("@studycentral.dk") &&
                u.Email != "teststudent@studycentral.dk"));
        Assert.Equal(5, await dbContext.Courses.CountAsync());
        Assert.Equal(100, await dbContext.Assignments.CountAsync());
        Assert.Equal(200, await dbContext.Announcements.CountAsync());
        Assert.Equal(300, await dbContext.Submissions.CountAsync());
        Assert.Equal(100, await dbContext.StudyFolders.CountAsync());
        Assert.Equal(200, await dbContext.StudyFiles.CountAsync());
        Assert.Equal(5, await dbContext.ChatRooms.CountAsync());
        Assert.Equal(1000, await dbContext.ChatMessages.CountAsync());

        Assert.Equal(4, await dbContext.Courses
            .CountAsync(c => c.TeacherId == CreateTestDataServiceData.Teacher1Id));

        var otherTeacherId = await dbContext.Users
            .Where(u => u.Role == UserRole.Teacher && u.Email == "maria.lund.teacher@studycentral.dk")
            .Select(u => u.Id)
            .SingleAsync();

        Assert.Equal(1, await dbContext.Courses.CountAsync(c => c.TeacherId == otherTeacherId));

        Assert.Equal(4, await dbContext.CourseStudents
            .CountAsync(cs => cs.StudentId == CreateTestDataServiceData.Student1Id));

        Assert.Equal(40, await dbContext.Submissions
            .CountAsync(s => s.StudentId == CreateTestDataServiceData.Student1Id));

        Assert.Equal(150, await dbContext.Announcements
            .Include(a => a.Course)
            .CountAsync(a => a.Course.TeacherId == CreateTestDataServiceData.Teacher1Id));

        var announcements = await dbContext.Announcements.ToListAsync();
        Assert.All(announcements, announcement =>
            Assert.True(announcement.CreatedAt < DateTime.UtcNow));

        Assert.True(announcements.Select(a => a.CreatedAt.Date).Distinct().Count() > 10);

        Assert.Equal(80, await dbContext.Assignments
            .Include(a => a.Course)
            .CountAsync(a => a.Course.TeacherId == CreateTestDataServiceData.Teacher1Id));

        Assert.Equal(80, await dbContext.StudyFolders
            .Include(f => f.Course)
            .CountAsync(f => f.Course.TeacherId == CreateTestDataServiceData.Teacher1Id));

        Assert.Equal(520, await dbContext.ChatMessages
            .CountAsync(m => m.SenderId == CreateTestDataServiceData.Student1Id));

        var teacherIds = await dbContext.Users
            .Where(u => u.Role == UserRole.Teacher)
            .Select(u => u.Id)
            .ToListAsync();

        var teacherChatMessages = await dbContext.ChatMessages
            .CountAsync(m => teacherIds.Contains(m.SenderId));

        Assert.True(teacherChatMessages < 500);

        var videoFiles = await dbContext.StudyFiles
            .CountAsync(f => CreateTestDataServiceData.VideoSourceFiles.Contains(f.BlobName));

        Assert.Equal(4, videoFiles);

        var files = await dbContext.StudyFiles.ToListAsync();
        Assert.All(files, file =>
        {
            Assert.False(file.BlobName.StartsWith("studycentral-", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(file.BlobName, CreateTestDataServiceData.AllowedSourceFiles);
            Assert.Equal(1, CountOwners(file));
        });

        Assert.True(files.Select(f => f.BlobName).Distinct().Count() >= 7);

        var duplicateSubmissions = await dbContext.Submissions
            .GroupBy(s => new { s.AssignmentId, s.StudentId })
            .Where(g => g.Count() > 1)
            .CountAsync();

        Assert.Equal(0, duplicateSubmissions);

        var duplicateEnrollments = await dbContext.CourseStudents
            .GroupBy(cs => new { cs.CourseId, cs.StudentId })
            .Where(g => g.Count() > 1)
            .CountAsync();

        Assert.Equal(0, duplicateEnrollments);

        var duplicateChatMembers = await dbContext.ChatRoomMembers
            .GroupBy(m => new { m.ChatRoomId, m.UserId })
            .Where(g => g.Count() > 1)
            .CountAsync();

        Assert.Equal(0, duplicateChatMembers);

        var duplicateAssignments = await dbContext.Assignments
            .GroupBy(a => new { a.CourseId, a.Name })
            .Where(g => g.Count() > 1)
            .CountAsync();

        Assert.Equal(0, duplicateAssignments);

        var maxFolderDepth = await GetMaxFolderDepth(dbContext);
        Assert.True(maxFolderDepth <= 5, $"Expected folder depth to be 5 or less, but found {maxFolderDepth}.");
    }

    [Fact]
    public async Task CreateCourse_WhenCalled_CreatesSmallCourseDemo()
    {
        // Arrange
        await using var dbContext = ContextGenerator.GetStudyDbContext();
        var service = new CreateTestDataService(dbContext, CreateEnvironment());

        // Act
        await service.CreateCourse();

        // Assert
        Assert.Equal(1, await dbContext.Users.CountAsync(u => u.Role == UserRole.Teacher));
        Assert.Equal(6, await dbContext.Users.CountAsync(u => u.Role == UserRole.Student));
        Assert.Equal(1, await dbContext.Courses.CountAsync());
        Assert.Equal(2, await dbContext.Assignments.CountAsync());
        Assert.Equal(3, await dbContext.Announcements.CountAsync());
        Assert.All(await dbContext.Announcements.ToListAsync(), announcement =>
            Assert.True(announcement.CreatedAt < DateTime.UtcNow));
        Assert.Equal(4, await dbContext.Submissions.CountAsync());
        Assert.Equal(5, await dbContext.StudyFolders.CountAsync());
        Assert.Equal(2, await dbContext.StudyFiles.CountAsync());
        Assert.Equal(1, await dbContext.ChatRooms.CountAsync());
        Assert.Equal(5, await dbContext.ChatMessages.CountAsync());
    }

    private static async Task SeedExistingLoginUsers(StudyDbContext dbContext)
    {
        dbContext.Users.AddRange(
            new User
            {
                Id = CreateTestDataServiceData.AdminId,
                Email = "admin@studycentral.dk",
                PasswordHash = "seed",
                FirstName = "Admin",
                LastName = "User",
                Role = UserRole.Admin
            },
            new User
            {
                Id = CreateTestDataServiceData.Teacher1Id,
                Email = "teacher@studycentral.dk",
                PasswordHash = "seed",
                FirstName = "Teacher",
                LastName = "User",
                Role = UserRole.Teacher
            },
            new User
            {
                Id = CreateTestDataServiceData.Student1Id,
                Email = "student@studycentral.dk",
                PasswordHash = "seed",
                FirstName = "Student",
                LastName = "User",
                Role = UserRole.Student
            },
            new User
            {
                Id = Guid.Parse("99999999-aaaa-bbbb-cccc-999999999999"),
                Email = "old.teacher@studycentral.dk",
                PasswordHash = "seed",
                FirstName = "Old",
                LastName = "Teacher",
                Role = UserRole.Teacher
            });

        await dbContext.SaveChangesAsync();
    }

    private static IWebHostEnvironment CreateEnvironment()
    {
        var environment = new Mock<IWebHostEnvironment>();
        environment
            .SetupGet(e => e.ContentRootPath)
            .Returns(GetApiProjectPath());

        return environment.Object;
    }

    private static string GetApiProjectPath()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "StudyCentral.sln")))
            directory = directory.Parent;

        if (directory == null)
            throw new DirectoryNotFoundException("Could not find StudyCentral.sln from the test output directory.");

        return Path.Combine(directory.FullName, "StudyCentral.API");
    }

    private static int CountOwners(StudyFile file)
    {
        var owners = 0;

        if (file.StudyFolderId.HasValue)
            owners++;

        if (file.AssignmentId.HasValue)
            owners++;

        if (file.AnnouncementId.HasValue)
            owners++;

        if (file.SubmissionId.HasValue)
            owners++;

        return owners;
    }

    private static async Task<int> GetMaxFolderDepth(StudyDbContext dbContext)
    {
        var folders = await dbContext.StudyFolders
            .Select(f => new
            {
                f.Id,
                f.ParentFolderId
            })
            .ToDictionaryAsync(f => f.Id, f => f.ParentFolderId);

        return folders.Keys.Select(folderId => GetFolderDepth(folderId, folders)).Max();
    }

    private static int GetFolderDepth(Guid folderId, Dictionary<Guid, Guid?> parentsByFolderId)
    {
        var depth = 1;
        var seen = new HashSet<Guid> { folderId };
        var currentId = folderId;

        while (parentsByFolderId.TryGetValue(currentId, out var parentId) && parentId.HasValue)
        {
            if (!seen.Add(parentId.Value))
                throw new InvalidOperationException("Cycle detected in generated folder data.");

            depth++;
            currentId = parentId.Value;
        }

        return depth;
    }
}
