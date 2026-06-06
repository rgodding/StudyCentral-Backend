using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Configurations;

public static class SeedData
{
    private static readonly Guid AdminId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static readonly Guid TeacherId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly Guid StudentId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    private static readonly Guid TestStudentId =
        Guid.Parse("44444444-4444-4444-4444-444444444444");

    private static readonly Guid CourseId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    private static readonly Guid AnnouncementId =
        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    private readonly static Guid AnnouncementWithFileId =
        Guid.Parse("abababab-abab-abab-abab-abababababab");

    private static readonly Guid AssignmentId =
        Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

    private static readonly Guid SubmissionId =
        Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

    private static readonly Guid TestSubmissionId =
        Guid.Parse("12121212-1212-1212-1212-121212121212");

    private static readonly Guid FolderId =
        Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

    private static readonly Guid ChildFolderId =
        Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

    private static readonly Guid StudyFileId =
        Guid.Parse("56565656-5656-5656-5656-565656565656");

    private static readonly Guid StudyFile2Id =
        Guid.Parse("67676767-6767-6767-6767-676767676767");
    
    private static readonly Guid FileAttachedToAnnouncementId =
        Guid.Parse("78787878-7878-7878-7878-787878787878");

    private static readonly Guid TestFolderId =
        Guid.Parse("abababab-abab-abab-abab-abababababab");

    private static readonly Guid TestAnnouncementId =
        Guid.Parse("cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd");

    private static readonly Guid TestAssignmentId =
        Guid.Parse("efefefef-efef-efef-efef-efefefefefef");

    private static readonly DateTime SeedDate =
        new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedCourses(modelBuilder);
        SeedEnrollments(modelBuilder);

        SeedAnnouncements(modelBuilder);
        SeedAssignments(modelBuilder);
        SeedSubmissions(modelBuilder);
        SeedFolders(modelBuilder);
        SeedFiles(modelBuilder);
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = AdminId,
                Email = "admin@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Admin",
                LastName = "User",
                Role = UserRole.Admin
            },
            new User
            {
                Id = TeacherId,
                Email = "teacher@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Teacher",
                LastName = "User",
                Role = UserRole.Teacher
            },
            new User
            {
                Id = StudentId,
                Email = "student@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Student",
                LastName = "User",
                Role = UserRole.Student
            },
            new User
            {
                Id = TestStudentId,
                Email = "teststudent@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Test",
                LastName = "Student",
                Role = UserRole.Student
            }
        );
    }

    private static void SeedCourses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = CourseId,
                Name = "System Integration",
                Description = "StudyCentral demonstration course",
                TeacherId = TeacherId
            }
        );
    }

    private static void SeedEnrollments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseStudent>().HasData(
            new CourseStudent
            {
                CourseId = CourseId,
                StudentId = StudentId
            },
            new CourseStudent
            {
                CourseId = CourseId,
                StudentId = TestStudentId
            }
        );
    }

    private static void SeedAnnouncements(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>().HasData(
            new Announcement
            {
                Id = AnnouncementId,
                Name = "Welcome Announcement",
                Content = "Welcome to StudyCentral",
                CourseId = CourseId
            },
            new Announcement
            {
                Id = TestAnnouncementId,
                Name = "Exam Information",
                Content = "The final exam will take place in June.",
                CourseId = CourseId
            },
            new Announcement
            {
                Id = AnnouncementWithFileId,
                Name = "Announcement with File",
                Content = "This announcement has an attached file.",
                CourseId = CourseId
            }
        );
    }

    private static void SeedAssignments(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>().HasData(
            new Assignment
            {
                Id = AssignmentId,
                Name = "Demo Assignment",
                Description = "Create a simple API",
                Deadline = SeedDate.AddDays(14),
                CourseId = CourseId
            },
            new Assignment
            {
                Id = TestAssignmentId,
                Name = "Database Assignment",
                Description = "Design and implement a relational database",
                Deadline = SeedDate.AddDays(21),
                CourseId = CourseId
            });
    }

    private static void SeedSubmissions(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>().HasData(
            new Submission
            {
                Id = SubmissionId,
                AssignmentId = AssignmentId,
                StudentId = StudentId,
                Comment = "Demo submission",
                Status = SubmissionStatus.Submitted,
                SubmittedAt = SeedDate
            },
            new Submission
            {
                Id = TestSubmissionId,
                AssignmentId = TestAssignmentId,
                StudentId = TestStudentId,
                Comment = "Test student submission",
                Status = SubmissionStatus.Submitted,
                SubmittedAt = SeedDate.AddDays(1)
            });
    }

    private static void SeedFolders(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyFolder>().HasData(
            new StudyFolder
            {
                Id = FolderId,
                Name = "Course Materials",
                CourseId = CourseId
            },
            new StudyFolder
            {
                Id = ChildFolderId,
                Name = "Week 1",
                CourseId = CourseId,
                ParentFolderId = FolderId
            },
            new StudyFolder
            {
                Id = TestFolderId,
                Name = "Assignments",
                CourseId = CourseId
            });
    }

    private static void SeedFiles(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyFile>().HasData(
            new StudyFile
            {
                Id = StudyFileId,
                FileName = "studycentral-testfile1.odt",
                BlobName = "studycentral-testfile1.odt",
                FileType = FileType.Document,
                ContentType = "application/vnd.oasis.opendocument.text",
                Size = 102400,
                AltText = "Course syllabus",
                StudyFolderId = FolderId,
                UploadedById = TeacherId
            },
            new StudyFile
            {
                Id = StudyFile2Id,
                FileName = "studycentral-testfile2.odt",
                BlobName = "studycentral-testfile2.odt",
                FileType = FileType.Document,
                ContentType =
                    "application/vnd.oasis.opendocument.text",
                Size = 204800,
                AltText = "Week 1 lecture slides",
                StudyFolderId = ChildFolderId,
                UploadedById = TeacherId
            },
            new StudyFile
            {
                Id = FileAttachedToAnnouncementId,
                FileName = "studycentral-testfile3.pdf",
                BlobName = "studycentral-testfile3.pdf",
                FileType = FileType.Pdf,
                ContentType = "application/pdf",
                Size = 102400,
                AltText = "Announcement file",
                AnnouncementId = AnnouncementWithFileId,
                UploadedById = TeacherId
            });
    }
}