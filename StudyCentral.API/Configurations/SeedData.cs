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

    public static void Seed(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedCourses(modelBuilder);
        SeedEnrollments(modelBuilder);
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
            }
        );
    }
}