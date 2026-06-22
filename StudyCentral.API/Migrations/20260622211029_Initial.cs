using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyCentral.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deadline = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChatRoomId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SenderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChatRoomMembers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChatRoomId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    JoinedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomMembers", x => new { x.UserId, x.ChatRoomId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeacherId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudyFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParentFolderId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyFolders_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyFolders_StudyFolders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "StudyFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseStudent",
                columns: table => new
                {
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudent", x => new { x.CourseId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_CourseStudent_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudyFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FileName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlobName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    AltText = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StudyFolderId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AnnouncementId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SubmissionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UploadedById = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyFiles_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudyFiles_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudyFiles_StudyFolders_StudyFolderId",
                        column: x => x.StudyFolderId,
                        principalTable: "StudyFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ProfilePictureId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_StudyFiles_ProfilePictureId",
                        column: x => x.ProfilePictureId,
                        principalTable: "StudyFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubmittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    GradedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "TeacherId", "UpdatedAt" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2607), "Course about relational databases", "Database Systems", null, null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "ProfilePictureId", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2477), "admin@studycentral.dk", "Admin", "User", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 2, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2481), "teacher@studycentral.dk", "Teacher", "User", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 1, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2484), "student@studycentral.dk", "Student", "User", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 0, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2485), "teststudent@studycentral.dk", "Test", "Student", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 0, null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2482), "teacher2@studycentral.dk", "Teacher2", "User", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 1, null },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2486), "testteacher@studycentral.dk", "Test", "Teacher", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 1, null },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2487), "teststudentwithoutcourse@studycentral.dk", "Test", "StudentWithoutCourse", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 0, null },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2488), "testsubmissionstudent1@studycentral.dk", "Test", "SubmissionStudent1", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 0, null },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2489), "testsubmissionstudent2@studycentral.dk", "Test", "SubmissionStudent2", "$2a$11$ykLhMftf0qTgiJAxVTAt/eGyXwEKWocNpyC/a3wwOywH/XRNcK2e2", null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Content", "CourseId", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"), "The group project will start next week.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2636), "Project Information", null });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "TeacherId", "UpdatedAt" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2604), "StudyCentral demonstration course", "System Integration", new Guid("22222222-2222-2222-2222-222222222222"), null });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Content", "CourseId", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("abababab-abab-abab-abab-abababababab"), "This announcement has an attached file.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2638), "Announcement with File", null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Welcome to StudyCentral", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2634), "Welcome Announcement", null },
                    { new Guid("cdcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd"), "The final exam will take place in June.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2637), "Exam Information", null }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Deadline", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2694), new DateTime(2026, 6, 19, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), "Create a simple API", "Demo Assignment", null },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddabc"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2702), null, "Submit a proposal for your group project", "Project Proposal", null },
                    { new Guid("efefefef-efef-efef-efef-efefefefefef"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2701), new DateTime(2026, 6, 27, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), "Design and implement a relational database", "Database Assignment", null }
                });

            migrationBuilder.InsertData(
                table: "ChatRooms",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Name", "Type", "UpdatedAt" },
                values: new object[] { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), "System Integration Chat", 1, null });

            migrationBuilder.InsertData(
                table: "CourseStudent",
                columns: new[] { "CourseId", "StudentId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("99999999-9999-9999-9999-999999999999") }
                });

            migrationBuilder.InsertData(
                table: "StudyFolders",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Name", "ParentFolderId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("abababab-abab-abab-abab-abababababab"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2740), "Assignments", null, null },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2737), "Course Materials", null, null }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "ChatRoomId", "Content", "CreatedAt", "DeletedAt", "EditedAt", "SenderId" },
                values: new object[,]
                {
                    { new Guid("16161616-1616-1616-1616-161616161616"), new Guid("15151515-1515-1515-1515-151515151515"), "Welcome to the System Integration course chat.", new DateTime(2026, 6, 22, 21, 11, 29, 272, DateTimeKind.Utc).AddTicks(7260), null, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("17171717-1717-1717-1717-171717171717"), new Guid("15151515-1515-1515-1515-151515151515"), "Thanks. Is the first assignment already available?", new DateTime(2026, 6, 22, 21, 12, 29, 272, DateTimeKind.Utc).AddTicks(7260), null, null, new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("18181818-1818-1818-1818-181818181818"), new Guid("15151515-1515-1515-1515-151515151515"), "Yes, you can find it under assignments.", new DateTime(2026, 6, 22, 21, 13, 29, 272, DateTimeKind.Utc).AddTicks(7260), null, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("19191919-1919-1919-1919-191919191919"), new Guid("15151515-1515-1515-1515-151515151515"), "Should we submit individually or as a group?", new DateTime(2026, 6, 22, 21, 14, 29, 272, DateTimeKind.Utc).AddTicks(7260), null, null, new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("20202020-2020-2020-2020-202020202020"), new Guid("15151515-1515-1515-1515-151515151515"), "This one should be submitted individually.", new DateTime(2026, 6, 22, 21, 15, 29, 272, DateTimeKind.Utc).AddTicks(7260), null, null, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "ChatRoomMembers",
                columns: new[] { "ChatRoomId", "UserId", "JoinedAt", "LastSeenAt" },
                values: new object[,]
                {
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null }
                });

            migrationBuilder.InsertData(
                table: "StudyFiles",
                columns: new[] { "Id", "AltText", "AnnouncementId", "AssignmentId", "BlobName", "ContentType", "CreatedAt", "FileName", "FileType", "Size", "StudyFolderId", "SubmissionId", "UpdatedAt", "UploadedById" },
                values: new object[,]
                {
                    { new Guid("56565656-5656-5656-5656-565656565656"), "Course syllabus", null, null, "studycentral-testfile1.odt", "application/vnd.oasis.opendocument.text", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2752), "studycentral-testfile1.odt", 4, 9172L, new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("78787878-7878-7878-7878-787878787878"), "Announcement file", new Guid("abababab-abab-abab-abab-abababababab"), null, "studycentral-testfile3.pdf", "application/pdf", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2758), "studycentral-testfile3.pdf", 3, 17797L, null, null, null, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "StudyFolders",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Name", "ParentFolderId", "UpdatedAt" },
                values: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2739), "Week 1", new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), null });

            migrationBuilder.InsertData(
                table: "Submissions",
                columns: new[] { "Id", "AssignmentId", "Comment", "CreatedAt", "Feedback", "Grade", "GradedAt", "Status", "StudentId", "SubmittedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("12121212-1212-1212-1212-121212121212"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Test student submission 1", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2719), null, null, null, 1, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 23, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("13131313-1313-1313-1313-131313131313"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Test student submission 2", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2720), null, 1, new DateTime(2026, 6, 25, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), 3, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 24, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("14141414-1414-1414-1414-141414141414"), new Guid("efefefef-efef-efef-efef-efefefefefef"), "Test student submission 3", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2723), null, 4, new DateTime(2026, 6, 27, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), 4, new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 6, 26, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Demo submission", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2715), null, null, null, 1, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 6, 22, 21, 10, 29, 272, DateTimeKind.Utc).AddTicks(7260), null }
                });

            migrationBuilder.InsertData(
                table: "StudyFiles",
                columns: new[] { "Id", "AltText", "AnnouncementId", "AssignmentId", "BlobName", "ContentType", "CreatedAt", "FileName", "FileType", "Size", "StudyFolderId", "SubmissionId", "UpdatedAt", "UploadedById" },
                values: new object[,]
                {
                    { new Guid("67676767-6767-6767-6767-676767676767"), "Week 1 lecture slides", null, null, "studycentral-testfile2.odt", "application/vnd.oasis.opendocument.text", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2757), "studycentral-testfile2.odt", 4, 9207L, new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), null, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("89898989-8989-8989-8989-898989898989"), "Submission 1 code", null, null, "studycentral-testfile4.odt", "application/vnd.oasis.opendocument.text", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2760), "studycentral-testfile4.odt", 4, 204800L, null, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("90909090-9090-9090-9090-909090909090"), "Submission 2 code", null, null, "studycentral-testfile5.pdf", "application/pdf", new DateTime(2026, 6, 22, 21, 10, 29, 447, DateTimeKind.Utc).AddTicks(2762), "studycentral-testfile5.pdf", 4, 409600L, null, new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), null, new Guid("33333333-3333-3333-3333-333333333333") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourseId",
                table: "Announcements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId_Name",
                table: "Assignments",
                columns: new[] { "CourseId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatRoomId",
                table: "ChatMessages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMembers_ChatRoomId",
                table: "ChatRoomMembers",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_CourseId",
                table: "ChatRooms",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_StudentId",
                table: "CourseStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFiles_AnnouncementId",
                table: "StudyFiles",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFiles_AssignmentId",
                table: "StudyFiles",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFiles_StudyFolderId",
                table: "StudyFiles",
                column: "StudyFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFiles_SubmissionId",
                table: "StudyFiles",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFiles_UploadedById",
                table: "StudyFiles",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFolders_CourseId",
                table: "StudyFolders",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFolders_ParentFolderId",
                table: "StudyFolders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId_StudentId",
                table: "Submissions",
                columns: new[] { "AssignmentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                table: "Submissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfilePictureId",
                table: "Users",
                column: "ProfilePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Courses_CourseId",
                table: "Announcements",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatRooms_ChatRoomId",
                table: "ChatMessages",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_SenderId",
                table: "ChatMessages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomMembers_ChatRooms_ChatRoomId",
                table: "ChatRoomMembers",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomMembers_Users_UserId",
                table: "ChatRoomMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRooms_Courses_CourseId",
                table: "ChatRooms",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Users_StudentId",
                table: "CourseStudent",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyFiles_Submissions_SubmissionId",
                table: "StudyFiles",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyFiles_Users_UploadedById",
                table: "StudyFiles",
                column: "UploadedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Courses_CourseId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyFolders_Courses_CourseId",
                table: "StudyFolders");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyFiles_Users_UploadedById",
                table: "StudyFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_StudentId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatRoomMembers");

            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "StudyFiles");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "StudyFolders");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Assignments");
        }
    }
}
