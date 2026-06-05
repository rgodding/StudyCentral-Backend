using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class UserServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserService _service;
    
    public UserServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        _mapper = MapperGenerator.GetMapper();
        _service = new UserService(
            _dbContext,
            _mapper,
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
    
    
    
}