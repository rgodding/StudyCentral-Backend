using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Data;

public static class CreateTestDataServiceData
{
    public static readonly Guid AdminId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Guid Teacher1Id =
        Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static readonly Guid Student1Id =
        Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly DateTime BaseDate = DateTime.UtcNow;
        // new(2026, 2, 2, 8, 15, 0, DateTimeKind.Utc);

    public const string Teacher1Key = "teacher_1";
    public const string Student1Key = "student_1";

    public static readonly HashSet<string> AllowedSourceFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "testaudio.mp3",
        "testdoc.odt",
        "testimage.jpg",
        "testimage.png",
        "testmovie.mov",
        "testpdf-notitle.pdf",
        "testpdf-title.pdf",
        "testtext.txt",
        "testvideo.mp4"
    };

    public static readonly HashSet<string> VideoSourceFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "testmovie.mov",
        "testvideo.mp4"
    };

    public static CreateTestDataSet CreateSingleCourseData()
    {
        return new CreateTestDataSet
        {
            Teachers =
            [
                new()
                {
                    Key = Teacher1Key,
                    Id = Teacher1Id,
                    FirstName = "Teacher",
                    LastName = "User",
                    Email = "teacher@studycentral.dk",
                    Password = "password"
                }
            ],
            Students =
            [
                new()
                {
                    Key = Student1Key,
                    Id = Student1Id,
                    FirstName = "Student",
                    LastName = "User",
                    Email = "student@studycentral.dk",
                    Password = "password"
                },
                new()
                {
                    Key = "single_student_01",
                    FirstName = "Aksel",
                    LastName = "Berg",
                    Email = "single.aksel.berg@studycentral.dk",
                    Password = "password"
                },
                new()
                {
                    Key = "single_student_02",
                    FirstName = "Nora",
                    LastName = "Holm",
                    Email = "single.nora.holm@studycentral.dk",
                    Password = "password"
                },
                new()
                {
                    Key = "single_student_03",
                    FirstName = "Elias",
                    LastName = "Vestergaard",
                    Email = "single.elias.vestergaard@studycentral.dk",
                    Password = "password"
                },
                new()
                {
                    Key = "single_student_04",
                    FirstName = "Maja",
                    LastName = "Kjeldsen",
                    Email = "single.maja.kjeldsen@studycentral.dk",
                    Password = "password"
                },
                new()
                {
                    Key = "single_student_05",
                    FirstName = "Victor",
                    LastName = "Hald",
                    Email = "single.victor.hald@studycentral.dk",
                    Password = "password"
                }
            ],
            Courses =
            [
                new()
                {
                    Key = "single_course_classroom_life",
                    Name = "Classroom Life Demo",
                    Description =
                        "A smaller course used for quick checks of lessons, assignments, files, submissions, and chat.",
                    TeacherKey = Teacher1Key,
                    StudentKeys =
                    [
                        Student1Key,
                        "single_student_01",
                        "single_student_02",
                        "single_student_03",
                        "single_student_04",
                        "single_student_05"
                    ],
                    Announcements =
                    [
                        new()
                        {
                            Key = "single_announcement_welcome",
                            Name = "Welcome and Weekly Rhythm",
                            Content =
                                "Welcome to the demo course. We will use announcements for lesson reminders, assignment notes, and small schedule changes."
                        },
                        new()
                        {
                            Key = "single_announcement_lab",
                            Name = "Lab Groups for Wednesday",
                            Content =
                                "On Wednesday we will work in pairs. Bring your notes from the first lecture and be ready to explain your approach."
                        },
                        new()
                        {
                            Key = "single_announcement_feedback",
                            Name = "Feedback Window",
                            Content =
                                "Feedback for the first assignment will be posted before Friday afternoon. Use it before starting the next task."
                        }
                    ],
                    Assignments =
                    [
                        new()
                        {
                            Key = "single_assignment_reflection",
                            Name = "Learning Reflection",
                            Description =
                                "Write a short reflection about what helped you understand the first topic and what still feels unclear.",
                            DeadlineOffsetDays = 5,
                            Submissions =
                            [
                                new()
                                {
                                    Key = "single_submission_student_1_reflection",
                                    StudentKey = Student1Key,
                                    Comment =
                                        "I explained how the class examples helped me connect the theory to the exercises.",
                                    SubmittedAtOffsetDays = 2,
                                    Grade = "B",
                                    Feedback = "Good reflection. Add one concrete question for the next class."
                                },
                                new()
                                {
                                    Key = "single_submission_aksel_reflection",
                                    StudentKey = "single_student_01",
                                    Comment =
                                        "I focused on the group discussion and the part where we compared two possible solutions.",
                                    SubmittedAtOffsetDays = 3,
                                    Grade = "C",
                                    Feedback = "Clear enough. Try to connect the discussion to the assignment criteria."
                                }
                            ]
                        },
                        new()
                        {
                            Key = "single_assignment_plan",
                            Name = "Mini Project Plan",
                            Description =
                                "Prepare a simple project plan with tasks, risks, and a first checkpoint.",
                            DeadlineOffsetDays = 9,
                            Submissions =
                            [
                                new()
                                {
                                    Key = "single_submission_student_1_plan",
                                    StudentKey = Student1Key,
                                    Comment =
                                        "I included the first draft of the plan and marked the areas where I need teacher feedback.",
                                    SubmittedAtOffsetDays = 6,
                                    Status = "Submitted"
                                },
                                new()
                                {
                                    Key = "single_submission_nora_plan",
                                    StudentKey = "single_student_02",
                                    Comment =
                                        "The plan includes milestones for research, implementation, and presentation practice.",
                                    SubmittedAtOffsetDays = 7,
                                    Grade = "A",
                                    Feedback = "Strong structure and realistic timing."
                                }
                            ]
                        }
                    ],
                    Folders =
                    [
                        new()
                        {
                            Key = "single_folder_materials",
                            Name = "Course Materials"
                        },
                        new()
                        {
                            Key = "single_folder_week_01",
                            Name = "Week 01 - Getting Started",
                            ParentFolderKey = "single_folder_materials"
                        },
                        new()
                        {
                            Key = "single_folder_week_01_exercises",
                            Name = "Exercises",
                            ParentFolderKey = "single_folder_week_01"
                        },
                        new()
                        {
                            Key = "single_folder_week_01_solutions",
                            Name = "Solution Notes",
                            ParentFolderKey = "single_folder_week_01_exercises"
                        },
                        new()
                        {
                            Key = "single_folder_week_01_teacher_notes",
                            Name = "Teacher Notes",
                            ParentFolderKey = "single_folder_week_01_solutions"
                        }
                    ],
                    Files =
                    [
                        new()
                        {
                            Key = "single_file_intro_pdf",
                            FileName = "classroom-life-introduction.pdf",
                            SourceFileName = "testpdf-title.pdf",
                            ContentType = "application/pdf",
                            FileType = FileType.Pdf.ToString(),
                            Size = 64688,
                            AltText = "Introduction handout for the demo course",
                            UploadedByKey = Teacher1Key,
                            OwnerType = CreateTestDataOwnerTypes.Folder,
                            OwnerKey = "single_folder_materials"
                        },
                        new()
                        {
                            Key = "single_file_reflection_submission",
                            FileName = "student-reflection-notes.txt",
                            SourceFileName = "testtext.txt",
                            ContentType = "text/plain",
                            FileType = FileType.Document.ToString(),
                            Size = 26,
                            AltText = "Short notes attached to the student reflection",
                            UploadedByKey = Student1Key,
                            OwnerType = CreateTestDataOwnerTypes.Submission,
                            OwnerKey = "single_submission_student_1_reflection"
                        }
                    ],
                    ChatMessages =
                    [
                        new()
                        {
                            SenderKey = Teacher1Key,
                            Content = "Welcome to the course chat. Use this space for questions that can help the whole class.",
                            CreatedAtOffsetMinutes = 1
                        },
                        new()
                        {
                            SenderKey = Student1Key,
                            Content = "Thanks. Should the first reflection mention the group exercise from today?",
                            CreatedAtOffsetMinutes = 3
                        },
                        new()
                        {
                            SenderKey = Teacher1Key,
                            Content = "Yes, that is a good example to include if it helped your understanding.",
                            CreatedAtOffsetMinutes = 5
                        },
                        new()
                        {
                            SenderKey = "single_student_02",
                            Content = "Can we upload a small planning document with the mini project plan?",
                            CreatedAtOffsetMinutes = 8
                        },
                        new()
                        {
                            SenderKey = Teacher1Key,
                            Content = "Yes, supporting documents are welcome when they make your plan easier to review.",
                            CreatedAtOffsetMinutes = 10
                        }
                    ]
                }
            ]
        };
    }
}

public static class CreateTestDataOwnerTypes
{
    public const string Folder = "Folder";
    public const string Assignment = "Assignment";
    public const string Announcement = "Announcement";
    public const string Submission = "Submission";
}

public sealed class CreateTestDataResult
{
    public int TeachersCreated { get; set; }
    public int StudentsCreated { get; set; }
    public int CoursesCreated { get; set; }
    public int EnrollmentsCreated { get; set; }
    public int AssignmentsCreated { get; set; }
    public int AnnouncementsCreated { get; set; }
    public int StudyFoldersCreated { get; set; }
    public int StudyFilesCreated { get; set; }
    public int SubmissionsCreated { get; set; }
    public int ChatRoomsCreated { get; set; }
    public int ChatRoomMembersCreated { get; set; }
    public int ChatMessagesCreated { get; set; }
}

public sealed class CreateTestDataSet
{
    public List<CreateTestDataUserSeed> Teachers { get; set; } = [];
    public List<CreateTestDataUserSeed> Students { get; set; } = [];
    public List<CreateTestDataCourseSeed> Courses { get; set; } = [];
}

public sealed class CreateTestDataUserSeed
{
    public string Key { get; set; } = string.Empty;
    public Guid? Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = "password";
}

public sealed class CreateTestDataCourseSeed
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TeacherKey { get; set; } = string.Empty;
    public List<string> StudentKeys { get; set; } = [];
    public List<CreateTestDataAssignmentSeed> Assignments { get; set; } = [];
    public List<CreateTestDataAnnouncementSeed> Announcements { get; set; } = [];
    public List<CreateTestDataFolderSeed> Folders { get; set; } = [];
    public List<CreateTestDataFileSeed> Files { get; set; } = [];
    public List<CreateTestDataChatMessageSeed> ChatMessages { get; set; } = [];
}

public sealed class CreateTestDataAssignmentSeed
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DeadlineOffsetDays { get; set; }
    public List<CreateTestDataSubmissionSeed> Submissions { get; set; } = [];
}

public sealed class CreateTestDataSubmissionSeed
{
    public string Key { get; set; } = string.Empty;
    public string StudentKey { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public int SubmittedAtOffsetDays { get; set; }
    public string? Status { get; set; }
    public string? Grade { get; set; }
    public string? Feedback { get; set; }
}

public sealed class CreateTestDataAnnouncementSeed
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public sealed class CreateTestDataFolderSeed
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ParentFolderKey { get; set; }
}

public sealed class CreateTestDataFileSeed
{
    public string Key { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string SourceFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string? AltText { get; set; }
    public string UploadedByKey { get; set; } = string.Empty;
    public string OwnerType { get; set; } = string.Empty;
    public string OwnerKey { get; set; } = string.Empty;
}

public sealed class CreateTestDataChatMessageSeed
{
    public string SenderKey { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CreatedAtOffsetMinutes { get; set; }
}
