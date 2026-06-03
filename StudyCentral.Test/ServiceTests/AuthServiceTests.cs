using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class AuthServiceTests
{
    
    /// -----------------
    /// Register tests
    /// -----------------

    // Functional test
    // This tests that a user can successfully register with valid data
    // It verifies that a user is created and a token is returned
    [Fact] 
    public async Task Register_ValidData_ReturnsSuccess()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var jwtService = JwtServiceGenerator.GetJwtService();
        var authService = new AuthService(dbContext, jwtService);

        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "testPassword",
            FirstName = "Tester",
            LastName = "Testerson"
        };

        // Act
        var result = await authService.Register(dto);
        var user = await dbContext.Users
            .SingleOrDefaultAsync(u => u.Email == dto.Email);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        
        Assert.NotNull(user);
        Assert.Equal(dto.FirstName, user.FirstName);
        Assert.Equal(dto.LastName, user.LastName);
        Assert.Equal(UserRole.Student, user.Role);
        
        Assert.NotEqual(dto.Password, user.PasswordHash);
        Assert.True(PasswordHelper.VerifyHash(dto.Password, user.PasswordHash));
    }
    
    // Functional test
    // This tests that a user cannot register with an email that already exists
    // It verifies that the service throws an InvalidOperationException when duplicate email is used
    [Fact]
    public async Task Register_ExistingEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var jwtService = JwtServiceGenerator.GetJwtService();
        var authService = new AuthService(dbContext, jwtService);

        var existingUser = new User
        {
            Email = "test@test.com",
            FirstName = "Existing",
            LastName = "User",
            Role = UserRole.Student,
            PasswordHash = PasswordHelper.HashPassword("password123")
        };

        dbContext.Users.Add(existingUser);
        await dbContext.SaveChangesAsync();

        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "newPassword",
            FirstName = "New",
            LastName = "User"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            authService.Register(dto));
    }
}