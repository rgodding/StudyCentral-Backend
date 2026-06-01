using Moq;
using StudyCentral.API.Models.Dtos.Users;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class UserServiceTests
{
    // Functional Test
    // This tests that an existing user can be retrieved by their id
    [Fact]
    public async Task GetUserById_UserExists_ReturnsUser()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var blobServiceMock = new Mock<IBlobService>();
        var service = new UserService(dbContext, blobServiceMock.Object);
        var user = TestUserFactory.Create();

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await service.GetCurrentUser(user.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.Email, result.Email);
    }
    
    // Functional test
    // Tests that a user that does not exist cannot be retrieved by id and throws a not-found exception
    [Fact]
    public async Task GetUserById_UserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var blobServiceMock = new Mock<IBlobService>();
        var service = new UserService(dbContext, blobServiceMock.Object);
        var nonExistentUserId = Guid.NewGuid();
        
        // Act + Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetCurrentUser(nonExistentUserId));
    }
    
    // Functional Test
    // Tests that a user gets updated
    [Fact]
    public async Task UpdateCurrentUser_UserExists_UpdatesUser()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var blobServiceMock = new Mock<IBlobService>();
        var service = new UserService(dbContext, blobServiceMock.Object);
        var user = TestUserFactory.Create();
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var updateDto = new UpdateUserDto
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName"
        };
        
        // Act
        var result = await service.UpdateCurrentUser(user.Id, updateDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(updateDto.FirstName, result.FirstName);
        Assert.Equal(updateDto.LastName, result.LastName);
    }
}