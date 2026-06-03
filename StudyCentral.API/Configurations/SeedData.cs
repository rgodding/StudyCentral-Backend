using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Configurations;

public static class SeedData
{
    // Users
    private static readonly Guid AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid Teacher1Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid Teacher2Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid Student1Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private static readonly Guid Student2Id = Guid.Parse("55555555-5555-5555-5555-555555555555");
    private static readonly Guid Student3Id = Guid.Parse("66666666-6666-6666-6666-666666666666");
    private static readonly Guid Student4Id = Guid.Parse("77777777-7777-7777-7777-777777777777");
    private static readonly Guid Student5Id = Guid.Parse("88888888-8888-8888-8888-888888888888");

    // Courses
    private static readonly Guid Course1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Course2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid Course3Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

    // Announcements
    private static readonly Guid Announcement1Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Announcement2Id = Guid.Parse("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Announcement3Id = Guid.Parse("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Announcement4Id = Guid.Parse("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Announcement5Id = Guid.Parse("55555555-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Announcement6Id = Guid.Parse("66666666-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    // Assignments
    private static readonly Guid Assignment1Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111111");
    private static readonly Guid Assignment2Id = Guid.Parse("aaaaaaaa-2222-2222-2222-222222222222");
    private static readonly Guid Assignment3Id = Guid.Parse("aaaaaaaa-3333-3333-3333-333333333333");
    private static readonly Guid Assignment4Id = Guid.Parse("aaaaaaaa-4444-4444-4444-444444444444");
    private static readonly Guid Assignment5Id = Guid.Parse("aaaaaaaa-5555-5555-5555-555555555555");
    private static readonly Guid Assignment6Id = Guid.Parse("aaaaaaaa-6666-6666-6666-666666666666");

    // Submissions
    private static readonly Guid Submission1Id = Guid.Parse("aaaaaaaa-7777-7777-7777-777777777777");
    private static readonly Guid Submission2Id = Guid.Parse("aaaaaaaa-8888-8888-8888-888888888888");
    private static readonly Guid Submission3Id = Guid.Parse("aaaaaaaa-9999-9999-9999-999999999999");
    private static readonly Guid Submission4Id = Guid.Parse("bbbbbbbb-1111-1111-1111-111111111111");
    private static readonly Guid Submission5Id = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222222");
    private static readonly Guid Submission6Id = Guid.Parse("bbbbbbbb-3333-3333-3333-333333333333");


    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedCourses(modelBuilder);
        SeedEnrollments(modelBuilder);
        SeedAnnouncements(modelBuilder);
        SeedAssignments(modelBuilder);
        SeedSubmissions(modelBuilder);
        SeedStudyFiles(modelBuilder);
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = AdminId,
                Email = "admin@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Mister",
                LastName = "Admin",
                Role = UserRole.Admin
            },
            new User
            {
                Id = Teacher1Id,
                Email = "teacher1@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Alice",
                LastName = "Smith",
                Role = UserRole.Teacher
            },
            new User
            {
                Id = Teacher2Id,
                Email = "teacher2@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Bob",
                LastName = "Johnson",
                Role = UserRole.Teacher
            },
            new User()
            {
                Id = Student1Id,
                Email = "student1@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Charlie",
                LastName = "Brown",
                Role = UserRole.Student
            },
            new User
            {
                Id = Student2Id,
                Email = "student2@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "David",
                LastName = "Wilson",
                Role = UserRole.Student
            },
            new User
            {
                Id = Student3Id,
                Email = "student3@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Emma",
                LastName = "Johnson",
                Role = UserRole.Student
            },
            new User
            {
                Id = Student4Id,
                Email = "student4@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Frank",
                LastName = "Miller",
                Role = UserRole.Student
            },
            new User
            {
                Id = Student5Id,
                Email = "student5@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Grace",
                LastName = "Taylor",
                Role = UserRole.Student
            }
        );
    }

    private static void SeedCourses(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = Course1Id,
                Title = "Web Development",
                Description = "Learn HTML, CSS, JavaScript and ASP.NET development.",
                TeacherId = Teacher1Id
            },
            new Course
            {
                Id = Course2Id,
                Title = "Database Systems",
                Description = "Learn relational databases, SQL and data modelling.",
                TeacherId = Teacher1Id
            },
            new Course
            {
                Id = Course3Id,
                Title = "System Integration",
                Description = "Learn APIs, Docker, Azure and distributed systems.",
                TeacherId = Teacher2Id
            }
        );
    }

    private static void SeedEnrollments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity("CourseUser").HasData(
            new
            {
                EnrolledCoursesId = Course1Id,
                StudentsId = Student1Id
            },
            new
            {
                EnrolledCoursesId = Course1Id,
                StudentsId = Student2Id
            },
            new
            {
                EnrolledCoursesId = Course1Id,
                StudentsId = Student3Id
            },
            new
            {
                EnrolledCoursesId = Course2Id,
                StudentsId = Student1Id
            },
            new
            {
                EnrolledCoursesId = Course2Id,
                StudentsId = Student4Id
            },
            new
            {
                EnrolledCoursesId = Course3Id,
                StudentsId = Student2Id
            },
            new
            {
                EnrolledCoursesId = Course3Id,
                StudentsId = Student3Id
            },
            new
            {
                EnrolledCoursesId = Course3Id,
                StudentsId = Student5Id
            }
        );
    }

    private static void SeedAnnouncements(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>().HasData(
            new Announcement
            {
                Id = Announcement1Id,
                Title = "Welcome to Web Development",
                Content = "Welcome to the course. Please review the syllabus and course materials.",
                CourseId = Course1Id,
            },
            new Announcement
            {
                Id = Announcement2Id,
                Title = "First Assignment Released",
                Content = "The HTML Portfolio assignment is now available.",
                CourseId = Course1Id,
            },
            new Announcement
            {
                Id = Announcement3Id,
                Title = "Database Project Information",
                Content = "Project requirements and grading criteria have been uploaded.",
                CourseId = Course2Id,
            },
            new Announcement
            {
                Id = Announcement4Id,
                Title = "Exam Preparation",
                Content = "Review SQL joins, normalization and indexing before the exam.",
                CourseId = Course2Id,
            },
            new Announcement
            {
                Id = Announcement5Id,
                Title = "Docker Setup Guide",
                Content = "Please install Docker Desktop before next week's exercises.",
                CourseId = Course3Id,
            },
            new Announcement
            {
                Id = Announcement6Id,
                Title = "Azure Assignment Released",
                Content = "The Azure Blob Storage assignment is now available.",
                CourseId = Course3Id,
            }
        );
    }

    private static void SeedAssignments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>().HasData(
            new Assignment
            {
                Id = Assignment1Id,
                Title = "HTML Portfolio",
                Description = "Create a personal portfolio website using HTML and CSS.",
                Deadline = new DateTime(2026, 10, 1),
                CourseId = Course1Id,
            },
            new Assignment
            {
                Id = Assignment2Id,
                Title = "ASP.NET Web API",
                Description = "Develop a RESTful API using ASP.NET Core.",
                Deadline = new DateTime(2026, 10, 15),
                CourseId = Course1Id,
            },
            new Assignment
            {
                Id = Assignment3Id,
                Title = "ER Diagram Design",
                Description = "Design an ER diagram for the provided business case.",
                Deadline = new DateTime(2026, 10, 5),
                CourseId = Course2Id,
            },
            new Assignment
            {
                Id = Assignment4Id,
                Title = "SQL Query Assignment",
                Description = "Write SQL queries to solve the provided tasks.",
                Deadline = new DateTime(2026, 10, 20),
                CourseId = Course2Id,
            },
            new Assignment
            {
                Id = Assignment5Id,
                Title = "Docker Deployment",
                Description = "Containerize and deploy the application using Docker.",
                Deadline = new DateTime(2026, 10, 10),
                CourseId = Course3Id,
            },
            new Assignment
            {
                Id = Assignment6Id,
                Title = "Azure Blob Storage",
                Description = "Implement file upload and storage using Azure Blob Storage.",
                Deadline = new DateTime(2026, 10, 25),
                CourseId = Course3Id,
            }
        );
    }

    private static void SeedSubmissions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>().HasData(
            new Submission
            {
                Id = Submission1Id,
                AssignmentId = Assignment1Id,
                StudentId = Student1Id,
                Comment = "My HTML portfolio submission.",
                Feedback = "Excellent work.",
                Grade = 95
            },
            new Submission
            {
                Id = Submission2Id,
                AssignmentId = Assignment1Id,
                StudentId = Student2Id,
                Comment = "Portfolio assignment completed.",
                Feedback = "Good structure and styling.",
                Grade = 85
            },
            new Submission
            {
                Id = Submission3Id,
                AssignmentId = Assignment1Id,
                StudentId = Student3Id,
                Comment = "My submission.",
                Feedback = null,
                Grade = null
            },
            new Submission
            {
                Id = Submission4Id,
                AssignmentId = Assignment2Id,
                StudentId = Student1Id,
                Comment = "ASP.NET API project.",
                Feedback = "Well implemented API.",
                Grade = 90
            },
            new Submission
            {
                Id = Submission5Id,
                AssignmentId = Assignment3Id,
                StudentId = Student4Id,
                Comment = "ER diagram attached.",
                Feedback = "Good normalization.",
                Grade = 88
            },
            new Submission
            {
                Id = Submission6Id,
                AssignmentId = Assignment5Id,
                StudentId = Student5Id,
                Comment = "Docker deployment completed.",
                Feedback = null,
                Grade = null
            }
        );
    }

    private static void SeedStudyFiles(ModelBuilder modelBuilder)
    {
    }
}