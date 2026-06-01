using Moq;
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

        var user = TestUserFactory.Create();

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var service = new UserService(dbContext, blobServiceMock.Object);
        
        // Act
        var result = await service.GetCurrentUser(user.Id);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.Email, result.Email);
    }
}