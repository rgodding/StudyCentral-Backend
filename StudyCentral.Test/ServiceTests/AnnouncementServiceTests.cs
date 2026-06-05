using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class AnnouncementServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly AnnouncementService _service;

    public AnnouncementServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        _mapper = MapperGenerator.GetMapper();
        _service = new AnnouncementService(_dbContext, _mapper);
    }

    // ----------------
    // GetAll
    // ----------------

    [Fact]
    public async Task GetAll_WhenAnnouncementsExist_ReturnsAllAnnouncements()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var announcements = new List<Announcement>
        {
            new Announcement
            {
                Id = Guid.NewGuid(),
                Name = "A1",
                Content = "Content 1",
                CourseId = course.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Announcement
            {
                Id = Guid.NewGuid(),
                Name = "A2",
                Content = "Content 2",
                CourseId = course.Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        _dbContext.Announcements.AddRange(announcements);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAll_WhenNoAnnouncements_ReturnsEmptyList()
    {
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
    public async Task GetById_WhenAnnouncementExists_ReturnsAnnouncement()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var announcement = new Announcement
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Content = "Content",
            CourseId = course.Id,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Announcements.Add(announcement);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetById(announcement.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(announcement.Id, result.Id);
        Assert.Equal(announcement.Name, result.Name);
    }

    [Fact]
    public async Task GetById_WhenAnnouncementDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetById(id));
    }

    // ----------------
    // Create
    // ----------------

    [Fact]
    public async Task Create_WhenValidData_ReturnsCreatedAnnouncement()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateAnnouncementDto
        {
            Name = "New Announcement",
            Content = "Important message",
            CourseId = course.Id
        };

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Content, result.Content);
        Assert.Equal(dto.CourseId, result.CourseId);
    }

    [Fact]
    public async Task Create_WhenCourseDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dto = new CreateAnnouncementDto
        {
            Name = "Invalid",
            Content = "Invalid",
            CourseId = Guid.NewGuid()
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Create(dto));
    }

    // ----------------
    // Update
    // ----------------

    [Fact]
    public async Task Update_WhenAnnouncementExists_ReturnsUpdatedAnnouncement()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var announcement = new Announcement
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Content = "Old Content",
            CourseId = course.Id,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Announcements.Add(announcement);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateAnnouncementDto
        {
            Name = "Updated Name",
            Content = "Updated Content"
        };

        // Act
        var result = await _service.Update(announcement.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Content, result.Content);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async Task Update_WhenAnnouncementDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dto = new UpdateAnnouncementDto
        {
            Name = "X",
            Content = "Y"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Update(Guid.NewGuid(), dto));
    }

    // ----------------
    // Delete
    // ----------------

    [Fact]
    public async Task Delete_WhenAnnouncementExists_DeletesAnnouncement()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var announcement = new Announcement
        {
            Id = Guid.NewGuid(),
            Name = "To Delete",
            Content = "Content",
            CourseId = course.Id,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Announcements.Add(announcement);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.Delete(announcement.Id);

        // Assert
        var exists = await _dbContext.Announcements
            .AnyAsync(a => a.Id == announcement.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task Delete_WhenAnnouncementDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Delete(id));
    }
}