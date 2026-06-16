using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Configurations;

public static class SeedData
{
    private static readonly Guid AdminId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static readonly Guid TeacherId =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly Guid TeacherWithoutCourseId =
        Guid.Parse("55555555-5555-5555-5555-555555555555");

    private static readonly Guid StudentId =
        Guid.Parse("33333333-3333-3333-3333-333333333333");

    private static readonly Guid TestStudentId =
        Guid.Parse("44444444-4444-4444-4444-444444444444");

    private static readonly Guid TestStudentWithoutCourseId =
        Guid.Parse("77777777-7777-7777-7777-777777777777");

    private static readonly Guid TestTeacherId =
        Guid.Parse("66666666-6666-6666-6666-666666666666");

    private static readonly Guid TestSubmissionStudent1Id =
        Guid.Parse("88888888-8888-8888-8888-888888888888");

    private static readonly Guid TestSubmissionStudent2Id =
        Guid.Parse("99999999-9999-9999-9999-999999999999");

    private static readonly Guid CourseId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    private static readonly Guid Course2Id =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad");

    private static readonly Guid AnnouncementId =
        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    private static readonly Guid Announcement2Id =
        Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccd");

    private readonly static Guid AnnouncementWithFileId =
        Guid.Parse("abababab-abab-abab-abab-abababababab");

    private static readonly Guid AssignmentId =
        Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
    
    private static readonly Guid Assignment2Id =
        Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddabc");

    private static readonly Guid SubmissionId =
        Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");

    private static readonly Guid TestSubmission1Id =
        Guid.Parse("12121212-1212-1212-1212-121212121212");

    private static readonly Guid TestSubmission2Id =
        Guid.Parse("13131313-1313-1313-1313-131313131313");

    private static readonly Guid TestSubmission3Id =
        Guid.Parse("14141414-1414-1414-1414-141414141414");

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
    
    private static readonly Guid FileAttachedToSubmission1Id =
        Guid.Parse("89898989-8989-8989-8989-898989898989");
    
    private static readonly Guid FileAttachedToSubmission2Id =
        Guid.Parse("90909090-9090-9090-9090-909090909090");

    private static readonly Guid TestFolderId =
        Guid.Parse("abababab-abab-abab-abab-abababababab");

    private static readonly Guid TestAnnouncementId =
        Guid.Parse("cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd");

    private static readonly Guid TestAssignmentId =
        Guid.Parse("efefefef-efef-efef-efef-efefefefefef");

    private static readonly DateTime SeedDate =
        new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    private static readonly Guid CourseChatRoomId =
        Guid.Parse("15151515-1515-1515-1515-151515151515");

    private static readonly Guid ChatMessage1Id =
        Guid.Parse("16161616-1616-1616-1616-161616161616");

    private static readonly Guid ChatMessage2Id =
        Guid.Parse("17171717-1717-1717-1717-171717171717");

    private static readonly Guid ChatMessage3Id =
        Guid.Parse("18181818-1818-1818-1818-181818181818");

    private static readonly Guid ChatMessage4Id =
        Guid.Parse("19191919-1919-1919-1919-191919191919");

    private static readonly Guid ChatMessage5Id =
        Guid.Parse("20202020-2020-2020-2020-202020202020");

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
                Id = TeacherWithoutCourseId,
                Email = "teacher2@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Teacher2",
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
            },
            new User
            {
                Id = TestTeacherId,
                Email = "testteacher@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Test",
                LastName = "Teacher",
                Role = UserRole.Teacher
            },
            new User
            {
                Id = TestStudentWithoutCourseId,
                Email = "teststudentwithoutcourse@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Test",
                LastName = "StudentWithoutCourse",
                Role = UserRole.Student
            },
            new User
            {
                Id = TestSubmissionStudent1Id,
                Email = "testsubmissionstudent1@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Test",
                LastName = "SubmissionStudent1",
                Role = UserRole.Student
            },
            new User
            {
                Id = TestSubmissionStudent2Id,
                Email = "testsubmissionstudent2@studycentral.dk",
                PasswordHash = "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2",
                FirstName = "Test",
                LastName = "SubmissionStudent2",
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
            },
            new Course
            {
                Id = Course2Id,
                Name = "Database Systems",
                Description = "Course about relational databases"
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
            },
            new CourseStudent
            {
                CourseId = CourseId,
                StudentId = TestSubmissionStudent1Id
            },
            new CourseStudent
            {
                CourseId = CourseId,
                StudentId = TestSubmissionStudent2Id
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
                Id = Announcement2Id,
                Name = "Project Information",
                Content = "The group project will start next week.",
                CourseId = Course2Id
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
            },
            new Assignment
            {
                Id = Assignment2Id,
                Name = "Project Proposal",
                Description = "Submit a proposal for your group project",
                CourseId = CourseId
            }
            );
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
                Id = TestSubmission1Id,
                AssignmentId = AssignmentId,
                StudentId = TestStudentId,
                Comment = "Test student submission 1",
                Status = SubmissionStatus.Submitted,
                SubmittedAt = SeedDate.AddDays(1)
            },
            new Submission
            {
                Id = TestSubmission2Id,
                AssignmentId = AssignmentId,
                StudentId = TestSubmissionStudent1Id,
                Comment = "Test student submission 2",
                Status = SubmissionStatus.Passed,
                Grade = GradeLetter.B,
                SubmittedAt = SeedDate.AddDays(2),
                GradedAt = SeedDate.AddDays(3)
            },
            new Submission
            {
                Id = TestSubmission3Id,
                AssignmentId = TestAssignmentId,
                StudentId = TestSubmissionStudent2Id,
                Comment = "Test student submission 3",
                Status = SubmissionStatus.Failed,
                Grade = GradeLetter.F,
                SubmittedAt = SeedDate.AddDays(4),
                GradedAt = SeedDate.AddDays(5)
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
                Size = 9172,
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
                ContentType = "application/vnd.oasis.opendocument.text",
                Size = 9207,
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
                Size = 17797,
                AltText = "Announcement file",
                AnnouncementId = AnnouncementWithFileId,
                UploadedById = TeacherId
            },
            new StudyFile
            {
                Id = FileAttachedToSubmission1Id,
                FileName = "studycentral-testfile4.odt",
                BlobName = "studycentral-testfile4.odt",
                FileType = FileType.Document,
                ContentType = "application/vnd.oasis.opendocument.text",
                Size = 204800,
                AltText = "Submission 1 code",
                SubmissionId = SubmissionId,
                UploadedById = StudentId
            },
            new StudyFile
            {
                Id = FileAttachedToSubmission2Id,
                FileName = "studycentral-testfile5.pdf",
                BlobName = "studycentral-testfile5.pdf",
                FileType = FileType.Document,
                ContentType = "application/pdf",
                Size = 409600,
                AltText = "Submission 2 code",
                SubmissionId = SubmissionId,
                UploadedById = StudentId
            }
            
            );
    }
    
    private static void SeedChat(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ChatRoom>().HasData(
        new ChatRoom
        {
            Id = CourseChatRoomId,
            Name = "System Integration Chat",
            Type = ChatRoomType.Course,
            CourseId = CourseId,
            CreatedAt = SeedDate
        });

    modelBuilder.Entity<ChatRoomMember>().HasData(
        new ChatRoomMember
        {
            ChatRoomId = CourseChatRoomId,
            UserId = TeacherId,
            JoinedAt = SeedDate
        },
        new ChatRoomMember
        {
            ChatRoomId = CourseChatRoomId,
            UserId = StudentId,
            JoinedAt = SeedDate
        },
        new ChatRoomMember
        {
            ChatRoomId = CourseChatRoomId,
            UserId = TestStudentId,
            JoinedAt = SeedDate
        },
        new ChatRoomMember
        {
            ChatRoomId = CourseChatRoomId,
            UserId = TestSubmissionStudent1Id,
            JoinedAt = SeedDate
        },
        new ChatRoomMember
        {
            ChatRoomId = CourseChatRoomId,
            UserId = TestSubmissionStudent2Id,
            JoinedAt = SeedDate
        });

    modelBuilder.Entity<ChatMessage>().HasData(
        new ChatMessage
        {
            Id = ChatMessage1Id,
            ChatRoomId = CourseChatRoomId,
            SenderId = TeacherId,
            Content = "Welcome to the System Integration course chat.",
            CreatedAt = SeedDate.AddMinutes(1)
        },
        new ChatMessage
        {
            Id = ChatMessage2Id,
            ChatRoomId = CourseChatRoomId,
            SenderId = StudentId,
            Content = "Thanks. Is the first assignment already available?",
            CreatedAt = SeedDate.AddMinutes(2)
        },
        new ChatMessage
        {
            Id = ChatMessage3Id,
            ChatRoomId = CourseChatRoomId,
            SenderId = TeacherId,
            Content = "Yes, you can find it under assignments.",
            CreatedAt = SeedDate.AddMinutes(3)
        },
        new ChatMessage
        {
            Id = ChatMessage4Id,
            ChatRoomId = CourseChatRoomId,
            SenderId = TestStudentId,
            Content = "Should we submit individually or as a group?",
            CreatedAt = SeedDate.AddMinutes(4)
        },
        new ChatMessage
        {
            Id = ChatMessage5Id,
            ChatRoomId = CourseChatRoomId,
            SenderId = TeacherId,
            Content = "This one should be submitted individually.",
            CreatedAt = SeedDate.AddMinutes(5)
        });
}
}