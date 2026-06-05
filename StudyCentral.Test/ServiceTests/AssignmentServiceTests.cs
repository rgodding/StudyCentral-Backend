using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;
using System.Security;
using StudyCentral.API.Middleware;

namespace StudyCentral.Test.ServiceTests;

public class AssignmentServiceTests
{
    // -----------------
    // GetAll tests
    // -----------------

    [Fact]
    public async Task GetAll_ReturnsAssignments()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);

        dbContext.Courses.Add(course);
        dbContext.Assignments.Add(assignment);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    // -----------------
    // GetById tests
    // -----------------

    [Fact]
    public async Task GetById_Existing_ReturnsAssignment()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);

        dbContext.Courses.Add(course);
        dbContext.Assignments.Add(assignment);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetById(assignment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignment.Id, result.Id);
    }

    [Fact]
    public async Task GetById_NotFound_ThrowsException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetById(Guid.NewGuid()));
    }

    // -----------------
    // Create tests
    // -----------------

    [Fact]
    public async Task Create_Valid_CreatesAssignment()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var course = TestCourseFactory.Create();

        course.TeacherId = teacher.Id;

        dbContext.Users.Add(teacher);
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        var dto = new CreateAssignmentDto
        {
            Title = "Test Assignment",
            Description = "Desc",
            CourseId = course.Id
        };

        // Act
        var result = await service.Create(teacher.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Title, result.Title);

        var dbItem = await dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Title == dto.Title);

        Assert.NotNull(dbItem);
    }

    [Fact]
    public async Task Create_DuplicateTitle_ThrowsConflict()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var course = TestCourseFactory.Create();

        course.TeacherId = teacher.Id;

        var assignment = TestAssignmentFactory.Create(
            title: "Duplicate",
            courseId: course.Id);

        dbContext.Users.Add(teacher);
        dbContext.Courses.Add(course);
        dbContext.Assignments.Add(assignment);
        await dbContext.SaveChangesAsync();

        var dto = new CreateAssignmentDto
        {
            Title = "Duplicate",
            CourseId = course.Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionMiddleware.ConflictException>(() =>
            service.Create(teacher.Id, dto));
    }

    [Fact]
    public async Task Create_WrongTeacher_ThrowsSecurityException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var otherTeacher = TestUserFactory.Create(role: UserRole.Teacher);

        var course = TestCourseFactory.Create();
        course.TeacherId = teacher.Id;

        dbContext.Users.AddRange(teacher, otherTeacher);
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        var dto = new CreateAssignmentDto
        {
            Title = "Test",
            CourseId = course.Id
        };

        // Act & Assert
        await Assert.ThrowsAsync<SecurityException>(() =>
            service.Create(otherTeacher.Id, dto));
    }

    // -----------------
    // Delete tests
    // -----------------

    [Fact]
    public async Task Delete_Valid_RemovesAssignment()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        var course = TestCourseFactory.Create();

        course.TeacherId = teacher.Id;

        var assignment = TestAssignmentFactory.Create(courseId: course.Id);

        dbContext.Users.Add(teacher);
        dbContext.Courses.Add(course);
        dbContext.Assignments.Add(assignment);
        await dbContext.SaveChangesAsync();

        // Act
        await service.Delete(teacher.Id, assignment.Id);

        // Assert
        var exists = await dbContext.Assignments
            .AnyAsync(a => a.Id == assignment.Id);

        Assert.False(exists);
    }

    // -----------------
    // GetByCourseId tests
    // -----------------

    [Fact]
    public async Task GetByCourseId_ReturnsAssignments()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);

        dbContext.Courses.Add(course);
        dbContext.Assignments.Add(assignment);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetByCourseId(course.Id);

        // Assert
        Assert.Single(result);
    }

    // -----------------
    // GetByStudentId tests
    // -----------------

    [Fact]
    public async Task GetByStudentId_ReturnsAssignments()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var service = new AssignmentService(dbContext, mapper);

        var student = TestUserFactory.Create(role: UserRole.Student);
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);

        dbContext.Users.Add(student);
        dbContext.Courses.Add(course);
        dbContext.Assignments.Add(assignment);

        dbContext.Submissions.Add(new Submission
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            AssignmentId = assignment.Id
        });

        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetByStudentId(student.Id);

        // Assert
        Assert.Single(result);
    }
}