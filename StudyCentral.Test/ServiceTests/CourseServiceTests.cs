using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class CourseServiceTests
{
    // -----------------
    // CreateCourse tests
    // -----------------

    [Fact]
    public async Task CreateCourse_ValidData_ReturnsCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        dbContext.Users.Add(teacher);
        await dbContext.SaveChangesAsync();

        var dto = new CreateCourseDto
        {
            Title = "Web Dev",
            Description = "Learn ASP.NET"
        };

        // Act
        var result = await service.CreateCourse(teacher.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Title, result.Title);

        var dbCourse = await dbContext.Courses.FirstOrDefaultAsync(c => c.Title == dto.Title);
        Assert.NotNull(dbCourse);
    }

    [Fact]
    public async Task CreateCourse_DuplicateTitle_ThrowsException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        dbContext.Users.Add(teacher);

        var existingCourse = TestCourseFactory.Create(title: "Web Dev");
        dbContext.Courses.Add(existingCourse);

        await dbContext.SaveChangesAsync();

        var dto = new CreateCourseDto
        {
            Title = "Web Dev"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateCourse(teacher.Id, dto));
    }

    // -----------------
    // GetCourse tests
    // -----------------

    [Fact]
    public async Task GetCourseById_ExistingCourse_ReturnsCourse()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var course = TestCourseFactory.Create();
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetCourseById(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
    }

    [Fact]
    public async Task GetCourseById_NotFound_ThrowsException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetCourseById(Guid.NewGuid()));
    }

    // -----------------
    // AddStudent tests
    // -----------------

    [Fact]
    public async Task AddStudentToCourse_Valid_AddsStudent()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var student = TestUserFactory.Create(role: UserRole.Student);
        var course = TestCourseFactory.Create();

        dbContext.Users.AddRange(student);
        course.TeacherId = teacher.Id;
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        // Act
        await service.AddStudentToCourse(teacher.Id, course.Id, student.Id);

        // Assert
        var exists = await dbContext.CourseStudents
            .AnyAsync(cs => cs.CourseId == course.Id && cs.StudentId == student.Id);

        Assert.True(exists);
    }

    [Fact]
    public async Task AddStudentToCourse_Duplicate_ThrowsException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var student = TestUserFactory.Create(role: UserRole.Student);
        var course = TestCourseFactory.Create();
        
        dbContext.Users.AddRange(teacher, student);
        dbContext.Courses.Add(course);
        
        course.TeacherId = teacher.Id;

        dbContext.CourseStudents.Add(new CourseStudent
        {
            CourseId = course.Id,
            StudentId = student.Id
        });

        await dbContext.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.AddStudentToCourse(teacher.Id, course.Id, student.Id));
    }

    // -----------------
    // RemoveStudent tests
    // -----------------

    [Fact]
    public async Task RemoveStudentFromCourse_Valid_RemovesStudent()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var student = TestUserFactory.Create(role: UserRole.Student);
        var course = TestCourseFactory.Create();

        dbContext.Users.AddRange(student);
        course.TeacherId = teacher.Id;
        dbContext.Courses.Add(course);

        dbContext.CourseStudents.Add(new CourseStudent
        {
            CourseId = course.Id,
            StudentId = student.Id
        });

        await dbContext.SaveChangesAsync();

        // Act
        await service.RemoveStudentFromCourse(teacher.Id, course.Id, student.Id);

        // Assert
        var exists = await dbContext.CourseStudents
            .AnyAsync(cs => cs.CourseId == course.Id && cs.StudentId == student.Id);

        Assert.False(exists);
    }

    // -----------------
    // IsStudentEnrolled tests
    // -----------------

    [Fact]
    public async Task IsStudentEnrolled_ReturnsTrue_WhenEnrolled()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var student = TestUserFactory.Create(role: UserRole.Student);
        var course = TestCourseFactory.Create();

        dbContext.Users.Add(student);
        dbContext.Courses.Add(course);

        dbContext.CourseStudents.Add(new CourseStudent
        {
            CourseId = course.Id,
            StudentId = student.Id
        });

        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.IsStudentEnrolled(course.Id, student.Id);

        // Assert
        Assert.True(result);
    }

    // -----------------
    // ClearCourseStudents tests
    // -----------------

    [Fact]
    public async Task ClearCourseStudents_RemovesAllStudents()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new CourseService(dbContext, mapper);

        var student1 = TestUserFactory.Create(role: UserRole.Student);
        var student2 = TestUserFactory.Create(role: UserRole.Student);
        var course = TestCourseFactory.Create();

        dbContext.Users.AddRange(student1, student2);
        dbContext.Courses.Add(course);

        dbContext.CourseStudents.AddRange(
            new CourseStudent { CourseId = course.Id, StudentId = student1.Id },
            new CourseStudent { CourseId = course.Id, StudentId = student2.Id }
        );

        await dbContext.SaveChangesAsync();

        // Act
        await service.ClearCourseStudents(course.Id);

        // Assert
        var count = await dbContext.CourseStudents
            .CountAsync(cs => cs.CourseId == course.Id);

        Assert.Equal(0, count);
    }
}