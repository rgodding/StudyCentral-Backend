using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class CourseStudentServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CourseStudentService _service;

    public CourseStudentServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        _mapper = MapperGenerator.GetMapper();
        _service = new CourseStudentService(_dbContext, _mapper);
    }

    // ----------------
    // Enroll Student
    // ----------------

    [Fact]
    public async Task EnrollStudent_WhenValidData_AddsEnrollment()
    {
        // Arrange
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Test Course",
            Description = "Desc"
        };

        _dbContext.Users.Add(student);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.EnrollStudent(student.Id, course.Id);

        // Assert
        var exists = await _dbContext.CourseStudents
            .AnyAsync(cs => cs.StudentId == student.Id && cs.CourseId == course.Id);

        Assert.True(exists);
    }

    [Fact]
    public async Task EnrollStudent_WhenStudentDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Test Course",
            Description = "Desc"
        };

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var fakeStudentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.EnrollStudent(fakeStudentId, course.Id));
    }

    [Fact]
    public async Task EnrollStudent_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        _dbContext.Users.Add(student);
        await _dbContext.SaveChangesAsync();

        var fakeCourseId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.EnrollStudent(student.Id, fakeCourseId));
    }

    [Fact]
    public async Task EnrollStudent_WhenAlreadyEnrolled_ThrowsInvalidOperationException()
    {
        // Arrange
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Test Course",
            Description = "Desc"
        };

        _dbContext.Users.Add(student);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        await _service.EnrollStudent(student.Id, course.Id);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.EnrollStudent(student.Id, course.Id));
    }

    [Fact]
    public async Task EnrollStudent_WhenUserIsNotStudent_ThrowsInvalidOperationException()
    {
        // Arrange
        var student = TestUserFactory.Create(role: UserRole.Teacher);
        var course = TestCourseFactory.Create();
        var enrollment = new CourseStudent
        {
            StudentId = student.Id,
            CourseId = course.Id
        };
        _dbContext.Users.Add(student);
        _dbContext.Courses.Add(course);
        _dbContext.Add(enrollment);
        await _dbContext.SaveChangesAsync();
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.EnrollStudent(student.Id, course.Id));       
    }

    // ----------------
    // Remove Student
    // ----------------

    [Fact]
    public async Task RemoveStudent_WhenEnrollmentExists_RemovesEnrollment()
    {
        // Arrange
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Test Course",
            Description = "Desc"
        };

        _dbContext.Users.Add(student);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        await _service.EnrollStudent(student.Id, course.Id);

        // Act
        await _service.RemoveStudent(student.Id, course.Id);

        // Assert
        var exists = await _dbContext.CourseStudents
            .AnyAsync(cs => cs.StudentId == student.Id && cs.CourseId == course.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task RemoveStudent_WhenEnrollmentDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var courseId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.RemoveStudent(studentId, courseId));
    }
}