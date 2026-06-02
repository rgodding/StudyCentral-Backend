using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudyCentral.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Deadline = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TeacherId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseUser",
                columns: table => new
                {
                    EnrolledCoursesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUser", x => new { x.EnrolledCoursesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseUser_Courses_EnrolledCoursesId",
                        column: x => x.EnrolledCoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlobName = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    AltText = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UploadedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UploadedById = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SubmissionId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Files_ProfilePictureId",
                        column: x => x.ProfilePictureId,
                        principalTable: "Files",
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
                    Feedback = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Grade = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AssignmentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StudentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "ProfilePictureId", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8140), "admin@mail.com", "Mister", "Admin", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 3, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8142) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8149), "teacher1@mail.com", "Alice", "Smith", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 2, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8149) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8150), "teacher2@mail.com", "Bob", "Johnson", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 2, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8151) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8152), "student1@mail.com", "Charlie", "Brown", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8152) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8154), "student2@mail.com", "David", "Wilson", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8154) },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8155), "student3@mail.com", "Emma", "Johnson", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8156) },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8157), "student4@mail.com", "Frank", "Miller", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8157) },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8158), "student5@mail.com", "Grace", "Taylor", "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6", null, 1, new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8158) }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedAt", "Description", "TeacherId", "Title" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8271), "Learn HTML, CSS, JavaScript and ASP.NET development.", new Guid("22222222-2222-2222-2222-222222222222"), "Web Development" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8275), "Learn relational databases, SQL and data modelling.", new Guid("22222222-2222-2222-2222-222222222222"), "Database Systems" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8276), "Learn APIs, Docker, Azure and distributed systems.", new Guid("33333333-3333-3333-3333-333333333333"), "System Integration" }
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Content", "CourseId", "CreatedAt", "CreatedById", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Welcome to the course. Please review the syllabus and course materials.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8315), new Guid("22222222-2222-2222-2222-222222222222"), "Welcome to Web Development", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "The HTML Portfolio assignment is now available.", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8317), new Guid("22222222-2222-2222-2222-222222222222"), "First Assignment Released", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Project requirements and grading criteria have been uploaded.", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8318), new Guid("22222222-2222-2222-2222-222222222222"), "Database Project Information", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Review SQL joins, normalization and indexing before the exam.", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8319), new Guid("22222222-2222-2222-2222-222222222222"), "Exam Preparation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("55555555-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Please install Docker Desktop before next week's exercises.", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8319), new Guid("33333333-3333-3333-3333-333333333333"), "Docker Setup Guide", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("66666666-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "The Azure Blob Storage assignment is now available.", new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8344), new Guid("33333333-3333-3333-3333-333333333333"), "Azure Assignment Released", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "CourseId", "CreatedAt", "CreatedById", "Deadline", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8360), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Create a personal portfolio website using HTML and CSS.", "HTML Portfolio" },
                    { new Guid("aaaaaaaa-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8364), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Develop a RESTful API using ASP.NET Core.", "ASP.NET Web API" },
                    { new Guid("aaaaaaaa-3333-3333-3333-333333333333"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8366), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Design an ER diagram for the provided business case.", "ER Diagram Design" },
                    { new Guid("aaaaaaaa-4444-4444-4444-444444444444"), new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8371), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Write SQL queries to solve the provided tasks.", "SQL Query Assignment" },
                    { new Guid("aaaaaaaa-5555-5555-5555-555555555555"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8372), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Containerize and deploy the application using Docker.", "Docker Deployment" },
                    { new Guid("aaaaaaaa-6666-6666-6666-666666666666"), new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8374), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Implement file upload and storage using Azure Blob Storage.", "Azure Blob Storage" }
                });

            migrationBuilder.InsertData(
                table: "CourseUser",
                columns: new[] { "EnrolledCoursesId", "StudentsId" },
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
                columns: new[] { "Id", "AssignmentId", "Comment", "Feedback", "Grade", "StudentId", "SubmittedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-7777-7777-7777-777777777777"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "My HTML portfolio submission.", "Excellent work.", 95m, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8392) },
                    { new Guid("aaaaaaaa-8888-8888-8888-888888888888"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "Portfolio assignment completed.", "Good structure and styling.", 85m, new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8398) },
                    { new Guid("aaaaaaaa-9999-9999-9999-999999999999"), new Guid("aaaaaaaa-1111-1111-1111-111111111111"), "My submission.", null, null, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8399) },
                    { new Guid("bbbbbbbb-1111-1111-1111-111111111111"), new Guid("aaaaaaaa-2222-2222-2222-222222222222"), "ASP.NET API project.", "Well implemented API.", 90m, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8400) },
                    { new Guid("bbbbbbbb-2222-2222-2222-222222222222"), new Guid("aaaaaaaa-3333-3333-3333-333333333333"), "ER diagram attached.", "Good normalization.", 88m, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8402) },
                    { new Guid("bbbbbbbb-3333-3333-3333-333333333333"), new Guid("aaaaaaaa-5555-5555-5555-555555555555"), "Docker deployment completed.", null, null, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 6, 2, 18, 6, 19, 227, DateTimeKind.Utc).AddTicks(8403) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourseId",
                table: "Announcements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CreatedById",
                table: "Announcements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CreatedById",
                table: "Assignments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_StudentsId",
                table: "CourseUser",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_AssignmentId",
                table: "Files",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CourseId",
                table: "Files",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_SubmissionId",
                table: "Files",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploadedById",
                table: "Files",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserId",
                table: "Files",
                column: "UserId");

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
                name: "FK_Announcements_Users_CreatedById",
                table: "Announcements",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_CreatedById",
                table: "Assignments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_Users_StudentsId",
                table: "CourseUser",
                column: "StudentsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Submissions_SubmissionId",
                table: "Files",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_UploadedById",
                table: "Files",
                column: "UploadedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Courses_CourseId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_CreatedById",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_UploadedById",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_UserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Users_StudentId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "CourseUser");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Assignments");
        }
    }
}
