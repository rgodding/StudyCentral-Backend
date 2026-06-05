using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyCentral.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
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
                name: "AnnouncementStudyFile",
                columns: table => new
                {
                    AnnouncementId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementStudyFile", x => new { x.AnnouncementId, x.FilesId });
                    table.ForeignKey(
                        name: "FK_AnnouncementStudyFile_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AssignmentStudyFile",
                columns: table => new
                {
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentStudyFile", x => new { x.AssignmentId, x.FilesId });
                    table.ForeignKey(
                        name: "FK_AssignmentStudyFile_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    UploadedById = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFiles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudyFileStudyFolder",
                columns: table => new
                {
                    FilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudyFolderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFileStudyFolder", x => new { x.FilesId, x.StudyFolderId });
                    table.ForeignKey(
                        name: "FK_StudyFileStudyFolder_StudyFiles_FilesId",
                        column: x => x.FilesId,
                        principalTable: "StudyFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyFileStudyFolder_StudyFolders_StudyFolderId",
                        column: x => x.StudyFolderId,
                        principalTable: "StudyFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "StudyFileSubmission",
                columns: table => new
                {
                    FilesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SubmissionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyFileSubmission", x => new { x.FilesId, x.SubmissionId });
                    table.ForeignKey(
                        name: "FK_StudyFileSubmission_StudyFiles_FilesId",
                        column: x => x.FilesId,
                        principalTable: "StudyFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyFileSubmission_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "ProfilePictureId", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1418), "admin@mail.com", "Mister", "Admin", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 2, null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1422), "teacher1@mail.com", "Alice", "Smith", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1424), "teacher2@mail.com", "Bob", "Johnson", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1425), "student1@mail.com", "Charlie", "Brown", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 0, null },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1427), "student2@mail.com", "David", "Wilson", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 0, null },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1428), "student3@mail.com", "Emma", "Johnson", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 0, null },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1448), "student4@mail.com", "Frank", "Miller", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 0, null },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1449), "student5@mail.com", "Grace", "Taylor", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "TeacherId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1529), "Learn HTML, CSS, JavaScript and ASP.NET development.", "Web Development", new Guid("22222222-2222-2222-2222-222222222222"), null },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1532), "Learn relational databases, SQL and data modelling.", "Database Systems", new Guid("22222222-2222-2222-2222-222222222222"), null },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1534), "Learn APIs, Docker, Azure and distributed systems.", "System Integration", new Guid("33333333-3333-3333-3333-333333333333"), null }
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Content", "CourseId", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Welcome to the course. Please review the syllabus and course materials.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1568), "Welcome to Web Development", null },
                    { new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "The HTML Portfolio assignment is now available.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1570), "First Assignment Released", null },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Project requirements and grading criteria have been uploaded.", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1572), "Database Project Information", null },
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Review SQL joins, normalization and indexing before the exam.", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1573), "Exam Preparation", null },
                    { new Guid("55555555-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Please install Docker Desktop before next week's exercises.", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1573), "Docker Setup Guide", null },
                    { new Guid("66666666-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "The Azure Blob Storage assignment is now available.", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1574), "Azure Assignment Released", null }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Deadline", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1590), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create a personal portfolio website using HTML and CSS.", "HTML Portfolio", null },
                    { new Guid("aaaaaaaa-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1594), new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Develop a RESTful API using ASP.NET Core.", "ASP.NET Web API", null },
                    { new Guid("aaaaaaaa-3333-3333-3333-333333333333"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1595), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Design an ER diagram for the provided business case.", "ER Diagram Design", null },
                    { new Guid("aaaaaaaa-4444-4444-4444-444444444444"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1597), new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Write SQL queries to solve the provided tasks.", "SQL Query Assignment", null },
                    { new Guid("aaaaaaaa-5555-5555-5555-555555555555"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1598), new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Containerize and deploy the application using Docker.", "Docker Deployment", null },
                    { new Guid("aaaaaaaa-6666-6666-6666-666666666666"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1599), new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Implement file upload and storage using Azure Blob Storage.", "Azure Blob Storage", null }
                });

            migrationBuilder.InsertData(
                table: "CourseStudent",
                columns: new[] { "CourseId", "StudentId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("88888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "Submissions",
                columns: new[] { "Id", "AssignmentId", "Comment", "CreatedAt", "Feedback", "Grade", "GradedAt", "Status", "StudentId", "SubmittedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-7777-7777-7777-777777777777"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "My HTML portfolio submission.", new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1615), "Excellent work.", 0, null, 0, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1615), null },
                    { new Guid("aaaaaaaa-8888-8888-8888-888888888888"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "Portfolio assignment completed.", new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1619), "Good structure and styling.", 1, null, 0, new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1618), null },
                    { new Guid("aaaaaaaa-9999-9999-9999-999999999999"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "My submission.", new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1620), null, 2, null, 0, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1620), null },
                    { new Guid("bbbbbbbb-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-2222-2222-2222-222222222222"), "ASP.NET API project.", new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1622), "Well implemented API.", 3, null, 0, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1621), null },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-3333-3333-3333-333333333333"), "ER diagram attached.", new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1623), "Good normalization.", 4, null, 0, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1623), null },
                    { new Guid("bbbbbbbb-3333-3333-3333-333333333333"), new Guid("aaaaaaaa-5555-5555-5555-555555555555"), "Docker deployment completed.", new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1624), null, null, null, 0, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 5, 20, 17, 28, 228, DateTimeKind.Utc).AddTicks(1624), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourseId",
                table: "Announcements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementStudyFile_FilesId",
                table: "AnnouncementStudyFile",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentStudyFile_FilesId",
                table: "AssignmentStudyFile",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_StudentId",
                table: "CourseStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFiles_UploadedById",
                table: "StudyFiles",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFileStudyFolder_StudyFolderId",
                table: "StudyFileStudyFolder",
                column: "StudyFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyFileSubmission_SubmissionId",
                table: "StudyFileSubmission",
                column: "SubmissionId");

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
                name: "FK_AnnouncementStudyFile_StudyFiles_FilesId",
                table: "AnnouncementStudyFile",
                column: "FilesId",
                principalTable: "StudyFiles",
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
                name: "FK_AssignmentStudyFile_StudyFiles_FilesId",
                table: "AssignmentStudyFile",
                column: "FilesId",
                principalTable: "StudyFiles",
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
                name: "FK_Users_StudyFiles_ProfilePictureId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AnnouncementStudyFile");

            migrationBuilder.DropTable(
                name: "AssignmentStudyFile");

            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "StudyFileStudyFolder");

            migrationBuilder.DropTable(
                name: "StudyFileSubmission");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "StudyFolders");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "StudyFiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
