using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class UserServiceTests
{
    /// -----------------
    /// GetAllUsers tests
    /// -----------------

    // This tests that all users are returned as previews
    // It verifies correct mapping and retrieval from the database
    [Fact]
    public async Task GetAllUsers_ExistingUsers_ReturnsUserPreviews()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        const int userCount = 10;
        var users = new List<User>();

        for (var i = 0; i < userCount; i++)
        {
            users.Add(TestUserFactory.Create());
        }
        dbContext.Users.AddRange(users);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userService.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(userCount, result.Count);
    }
    
    // Functional test
    // This tests that empty database returns empty list
    // It verifies safe handling of no data state
    [Fact]
    public async Task GetAllUsers_NoUsers_ReturnsEmptyList()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        // Act
        var result = await userService.GetAllUsers();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    /// -----------------
    /// GetUserById tests
    /// -----------------
    
    // Functional test
    // This tests that a user can be retrieved by their ID
    // It verifies correct mapping from entity to DTO
    [Fact]
    public async Task GetUserById_ExistingUser_ReturnsUser()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var user = TestUserFactory.Create();

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userService.GetUserById(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
    }

    // Functional test
    // This tests that requesting a non-existing user throws an exception
    // It verifies correct error handling for invalid IDs
    [Fact]
    public async Task GetUserById_NonExistingUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var randomId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => userService.GetUserById(randomId)
        );
    }

}