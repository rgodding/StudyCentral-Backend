using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class StudyFileServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly StudyFileService _service;

    public StudyFileServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        _service = new StudyFileService(
            _dbContext,
            new BlobServiceGenerator(),
            MapperGenerator.GetMapper());
    }

    [Fact]
    public async Task UploadFile_WhenUserExists_ReturnsCreatedStudyFile()
    {
        // Arrange
        var user = TestUserFactory.Create();
        var formFile = TestFileFactory.CreateTextFile("notes.txt");

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.UploadFile(
            formFile,
            user.Id,
            "Lecture notes");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("notes.txt", result.FileName);
        Assert.Equal("text/plain", result.ContentType);
        Assert.Equal(FileType.Document, result.FileType);
        Assert.Equal(user.Id, result.UploadedById);
        Assert.Equal("Lecture notes", result.AltText);
        Assert.True(await _dbContext.StudyFiles.AnyAsync(f => f.Id == result.Id));
    }

    [Fact]
    public async Task UploadFile_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var formFile = TestFileFactory.CreateTextFile();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UploadFile(formFile, Guid.NewGuid()));

        Assert.Empty(_dbContext.StudyFiles);
    }
}
