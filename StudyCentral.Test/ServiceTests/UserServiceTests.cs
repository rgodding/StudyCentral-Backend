using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class UserServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        _service = new UserService(
            _dbContext,
            mapper,
            new StudyFileServiceGenerator());
    }

    // ----------------
    // GetAll
    // ----------------

    [Fact]
    public async Task GetAll_WhenUsersExist_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            TestUserFactory.Create(),
            TestUserFactory.Create(),
            TestUserFactory.Create(),
            TestUserFactory.Create(),
            TestUserFactory.Create()
        };

        _dbContext.AddRange(users);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count);
    }

    [Fact]
    public async Task GetAll_WhenNoUsers_ReturnsEmptyList()
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
    public async Task GetById_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = TestUserFactory.Create(id: userId);

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetById(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.Role, result.Role);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetById(userId));
    }

    // ----------------
    // Create
    // ----------------

    [Fact]
    public async Task Create_WhenValidData_ReturnsCreatedUser()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = $"test{Guid.NewGuid()}@example.com",
            Password = "testPassword",
            Role = UserRole.Student
        };

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.Role, result.Role);
    }

    [Fact]
    public async Task Create_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var email = $"test{Guid.NewGuid()}@example.com";
        var existingUser = TestUserFactory.Create(email: email);

        _dbContext.Add(existingUser);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateUserDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Password = "testPassword",
            Role = UserRole.Student
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(dto));
    }

    // ----------------
    // Update
    // ----------------
    [Fact]
    public async Task Update_WhenUserExists_ReturnsUpdatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = TestUserFactory.Create(id: userId);

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateUserDto
        {
            FirstName = "Updated",
            LastName = "User",
            Email = $"updated{Guid.NewGuid()}@example.com"
        };

        // Act
        var result = await _service.Update(userId, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task Update_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new UpdateUserDto
        {
            FirstName = "Updated",
            LastName = "User",
            Email = $"updated{Guid.NewGuid()}@example.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.Update(userId, dto));
    }

    [Fact]
    public async Task Update_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingEmail = $"existing{Guid.NewGuid()}@example.com";

        var user = TestUserFactory.Create(id: userId);
        var existingUser = TestUserFactory.Create(email: existingEmail);

        _dbContext.AddRange(user, existingUser);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateUserDto
        {
            FirstName = "Updated",
            LastName = "User",
            Email = existingEmail
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Update(userId, dto));
    }

    [Theory]
    [InlineData("new@mail.com", "John", "Doe")]
    [InlineData(null, "UpdatedFirst", null)]
    [InlineData("updated@mail.com", null, "UpdatedLast")]
    [InlineData(null, null, null)]
    public async Task Update_WhenVariousInputs_AppliesCorrectChanges(
        string? newEmail,
        string? newFirstName,
        string? newLastName)
    {
        // Arrange
        var user = new User
        {
            Email = "original@mail.com",
            FirstName = "Original",
            LastName = "User",
            PasswordHash = "hash",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateUserDto
        {
            Email = newEmail,
            FirstName = newFirstName,
            LastName = newLastName
        };

        // Act
        var result = await _service.Update(user.Id, dto);

        // Reload from DB to verify persistence
        var updated = await _dbContext.Users.FirstAsync(u => u.Id == user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newEmail ?? "original@mail.com", updated.Email);
        Assert.Equal(newFirstName ?? "Original", updated.FirstName);
        Assert.Equal(newLastName ?? "User", updated.LastName);
    }

    // ----------------
    // Delete
    // ----------------

    [Fact]
    public async Task Delete_WhenUserExists_DeletesUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = TestUserFactory.Create(id: userId);

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.Delete(userId);

        // Assert
        var deletedUser = await _dbContext.Users.FindAsync(userId);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task Delete_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.Delete(userId));
    }

    // ----------------
    // GetMe
    // ----------------

    [Fact]
    public async Task GetMe_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = TestUserFactory.Create(id: userId);

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetMe(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.Role, result.Role);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetMe_WhenDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetMe(userId));
    }

    // -----------------
    // UpdateMe
    // -----------------

    [Fact]
    public async Task UpdateMe_WhenUserExists_ReturnsUpdatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = TestUserFactory.Create(id: userId);

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateUserDto
        {
            FirstName = "Updated",
            LastName = "User",
            Email = $"updated{Guid.NewGuid()}@example.com"
        };

        // Act
        var result = await _service.UpdateMe(userId, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task UpdateMe_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new UpdateUserDto
        {
            FirstName = "Updated",
            LastName = "User",
            Email = $"updated{Guid.NewGuid()}@example.com"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateMe(userId, dto));
    }

    [Theory]
    [InlineData("updatedFirstName", "updatedLastName", "updated@example.com")]
    [InlineData(null, "updatedLastName", null)]
    [InlineData("updatedFirstName", null, null)]
    [InlineData(null, null, null)]
    [InlineData(null, null, "updated@example.com")]
    public async Task UpdateMe_WhenVariousInputs_AppliesCorrectChanges(
        string? newFirstName,
        string? newLastName,
        string? newEmail)
    {
        // Arrange
        var user = TestUserFactory.Create(firstName: "OriginalFirst", lastName: "OriginalLast", email: "original@example.com");

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateUserDto
        {
            FirstName = newFirstName,
            LastName = newLastName,
            Email = newEmail
        };

        // Act
        var result = await _service.UpdateMe(user.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newFirstName ?? "OriginalFirst", result.FirstName);
        Assert.Equal(newLastName ?? "OriginalLast", result.LastName);
        Assert.Equal(newEmail ?? "original@example.com", result.Email);
    }


    // -----------------
    // UpdateMe
    // -----------------

    [Fact]
    public async Task ChangePassword_WhenValidData_ChangesPassword()
    {
        // Arrange
        var oldPassword = PasswordHelper.HashPassword("oldPassword");
        var user = TestUserFactory.Create(passwordHash: oldPassword);
        
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "oldPassword",
            NewPassword = "newPassword"
        };
        
        // Act
        await _service.ChangePassword(user.Id, dto);
        
        // Assert
        var updatedUser = await _dbContext.Users.FindAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.True(PasswordHelper.VerifyHash("newPassword", updatedUser.PasswordHash));
    }
    
    [Fact]
    public async Task ChangePassword_WhenCurrentPasswordIsIncorrect_ThrowsUnauthorizedAccessException(){
        // Arrange
        var user = TestUserFactory.Create(passwordHash: PasswordHelper.HashPassword("correctPassword"));
        
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "wrongPassword",
            NewPassword = "newPassword"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.ChangePassword(user.Id, dto));
    }
    
    [Fact]
    public async Task ChangePassword_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "anyPassword",
            NewPassword = "newPassword"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ChangePassword(userId, dto));
    }
    
    
    
    
    
}