using AutoMapper;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class AssignmentServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly AssignmentService _service;

    public AssignmentServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        IStudyFileService fileService = new StudyFileServiceGenerator();
        _service = new AssignmentService(_dbContext, mapper, fileService);
    }

    // ----------------
    // GetAll
    // ----------------

    [Fact]
    public async Task GetAll_WhenAssignmentsExist_ReturnsAllAssignments()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        var assignments = new List<Assignment>
        {
            TestAssignmentFactory.Create(courseId: course.Id),
            TestAssignmentFactory.Create(courseId: course.Id),
            TestAssignmentFactory.Create(courseId: course.Id)
        };

        _dbContext.AddRange(assignments);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignments.Count, result.Count);
    }

    [Fact]
    public async Task GetAll_WhenNoAssignments_ReturnsEmptyList()
    {
        // Arrange

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    // ----------------
    // GetById
    // ----------------

    [Fact]
    public async Task GetById_WhenAssignmentExists_ReturnsAssignment()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();
        
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        _dbContext.Add(assignment);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetById(assignment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignment.Id, result.Id);
        Assert.Equal(assignment.Name, result.Name);
        Assert.Equal(assignment.Description, result.Description);
    }

    [Fact]
    public async Task GetById_WhenAssignmentDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetById(nonExistentId));
    }

    // ----------------
    // Create
    // ----------------

    [Fact]
    public async Task Create_WhenValidData_ReturnsCreatedAssignment()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();
        
        var dto = new CreateAssignmentDto
        {
            Name = "Test Assignment",
            Description = "Test Description",
            CourseId = course.Id
        };

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);
    }

    [Fact]
    public async Task Create_WhenDescriptionIsNull_ReturnsCreatedAssignment()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();
        
        var dto = new CreateAssignmentDto
        {
            Name = "Test Assignment",
            Description = null,
            CourseId = course.Id
        };

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Null(result.Description);
    }

    // ----------------
    // Update
    // ----------------

    [Fact]
    public async Task Update_WhenAssignmentExists_ReturnsUpdatedAssignment()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        _dbContext.Add(assignment);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateAssignmentDto
        {
            Name = "Updated Name",
            Description = "Updated Description"
        };

        // Act
        var result = await _service.Update(assignment.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignment.Id, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task Update_WhenAssignmentDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dto = new UpdateAssignmentDto
        {
            Name = "Updated Name",
            Description = "Updated Description"
        };

        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Update(nonExistentId, dto));
    }

    [Theory]
    [InlineData(null, "Updated Description")]
    [InlineData("Updated Name", null)]
    public async Task Update_WhenValidCombination_ReturnsUpdatedAssignment(
        string? newName,
        string? newDescription)
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        
        const string name = "Original Name";
        const string description = "Original Description";

        var assignment = TestAssignmentFactory.Create(
            name: name,
            description: description,
            courseId: course.Id);

        _dbContext.Add(assignment);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateAssignmentDto
        {
            Name = newName,
            Description = newDescription
        };

        // Act
        var result = await _service.Update(assignment.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(assignment.Id, result.Id);
        Assert.Equal(newName ?? name, result.Name);
        Assert.Equal(newDescription ?? description, result.Description);
        Assert.NotNull(result.UpdatedAt);
    }

    // ----------------
    // Delete
    // ----------------

    [Fact]
    public async Task Delete_WhenAssignmentExists_DeletesAssignment()
    {
        // Arrange
        var assignment = TestAssignmentFactory.Create();
        _dbContext.Add(assignment);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.Delete(assignment.Id);

        // Assert
        var deleted = await _dbContext.Assignments.FindAsync(assignment.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Delete_WhenAssignmentDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Delete(nonExistentId));
    }
}