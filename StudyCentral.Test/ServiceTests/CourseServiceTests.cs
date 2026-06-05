using AutoMapper;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class CourseServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly CourseService _service;

    public CourseServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        _mapper = MapperGenerator.GetMapper();
        _service = new CourseService(_dbContext, _mapper);
    }

    // ----------------
    // GetAll
    // ----------------
    [Fact]
    public async Task GetAll_WhenCoursesExist_ReturnsAllCourses()
    {
        // Arrange
        var courses = new List<Course>
        {
            TestCourseFactory.Create(),
            TestCourseFactory.Create(),
            TestCourseFactory.Create(),
            TestCourseFactory.Create(),
            TestCourseFactory.Create()
        };
        
        _dbContext.AddRange(courses);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
    }
    
    [Fact]
    public async Task GetAll_WhenNoCourses_ReturnsEmptyList()
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
    public async Task GetById_WhenCourseExists_ReturnsCourse()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetById(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(course.Name, result.Name);
        Assert.Equal(course.Description, result.Description);
    }
    
    [Fact]
    public async Task GetById_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentCourseId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetById(nonExistentCourseId));
    }

    // ----------------
    // Create
    // ----------------
    
    [Fact]
    public async Task Create_WhenValidData_ReturnsCreatedCourse()
    {
        // Arrange
        var dto = new CreateCourseDto
        {
            Name = "Test Course",
            Description = "This is a test course."
        };

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);
    }
    
    [Fact]
    public async Task Create_WhenDescriptionIsNull_ReturnsCreatedCourse()
    {
        // Arrange
        var dto = new CreateCourseDto
        {
            Name = "Test Course",
            Description = null
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
    public async Task Update_WhenCourseExists_ReturnsUpdatedCourse()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var course = TestCourseFactory.Create(id: courseId);
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();
        
        var dto = new UpdateCourseDto
        {
            Name = "Updated Course Name",
            Description = "Updated Course Description"
         };

        // Act
        var result = await _service.Update(course.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);
        Assert.NotNull(result.UpdatedAt);
    }
    
    [Fact]
    public async Task Update_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentCourseId = Guid.NewGuid();
        var dto = new UpdateCourseDto
        {
            Name = "Updated Course Name",
            Description = "Updated Course Description"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.Update(nonExistentCourseId, dto));
    }
   

    [Theory]
    [InlineData(null, "Updated Course Name")]
    [InlineData("Updated Course Name", null)]
    public async Task Update_WhenValidCombination_ReturnsUpdatedCourse(string? newName, string? newDescription)
    {
        // Arrange
        const string name = "course name";
        const string description = "course description";
        
        var course = TestCourseFactory.Create(name: name, description: description);
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateCourseDto
        {
            Name = newName,
            Description = newDescription
        };
        
        // Act
        var result = await _service.Update(course.Id, dto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal(newName ?? name, result.Name);
        Assert.Equal(newDescription ?? description, result.Description);
        Assert.NotNull(result.UpdatedAt);
    }

    // ----------------
    // Delete
    // ----------------

    [Fact]
    public async Task Delete_WhenCourseExists_DeletesCourse()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();
        
        // Act
        await _service.Delete(course.Id);
        
        // Assert
        var deletedCourse = await _dbContext.Courses.FindAsync(course.Id);
        Assert.Null(deletedCourse);
    }
    
    [Fact]
    public async Task Delete_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var nonExistentCourseId = Guid.NewGuid();
        var course = TestCourseFactory.Create();
        _dbContext.Add(course);
        await _dbContext.SaveChangesAsync();
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.Delete(nonExistentCourseId));
    }

}
