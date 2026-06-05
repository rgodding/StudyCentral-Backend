using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.DTOs.User;
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
        await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.GetUserById(randomId)
        );
    }

    /// -----------------
    /// CreateUser tests
    /// -----------------

    // Functional test
    // This tests that a user can be successfully created
    // It verifies that the user is stored in the database and correctly mapped to a DTO
    [Fact]
    public async Task CreateUser_ValidData_ReturnsCreatedUser()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var dto = new CreateUserDto
        {
            Email = "test@test.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Role = UserRole.Student
        };

        // Act
        var result = await userService.CreateUser(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);

        var dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        Assert.NotNull(dbUser);
    }

    // Functional test
    // This tests that duplicate emails are not allowed
    // It verifies that the system prevents creation of users with existing emails
    [Fact]
    public async Task CreateUser_DuplicateEmail_ThrowsException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var existingUser = TestUserFactory.Create();
        dbContext.Users.Add(existingUser);
        await dbContext.SaveChangesAsync();

        var dto = new CreateUserDto
        {
            Email = existingUser.Email,
            FirstName = "John",
            LastName = "Doe"
        };

        // Act & Assert 
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => userService.CreateUser(dto)
        );

        Assert.NotNull(exception);
        Assert.Equal("Email already exists", exception.Message);
    }

    /// -----------------
    /// UpdateUser tests
    /// -----------------

    // Functional test
    // This tests that a user can be successfully updated
    // It verifies that the database values are modified and correctly persisted
    [Fact]
    public async Task UpdateUser_ValidData_UpdatesUserSuccessfully()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var user = TestUserFactory.Create(email: "updated@mail.com");

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new UpdateUserDto
        {
            Email = "updated@mail.com",
            FirstName = "Updated",
            LastName = "User"
        };

        // Act
        var result = await userService.UpdateUser(user.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);

        var dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        Assert.NotNull(dbUser);
        Assert.Equal(dto.Email, dbUser.Email);
        Assert.Equal(dto.FirstName, dbUser.FirstName);
        Assert.Equal(dto.LastName, dbUser.LastName);
    }

    // Functional test
    // This tests that updating a non-existing user throws an exception
    // It verifies correct error handling for invalid user IDs
    [Fact]
    public async Task UpdateUser_NonExistingUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var dto = new UpdateUserDto
        {
            Email = "updated@mail.com",
            FirstName = "Updated",
            LastName = "User"
        };

        var randomId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.UpdateUser(randomId, dto)
        );
    }

    /// -----------------
    /// DeleteUser tests
    /// -----------------
    // Functional test
    // This tests that a user can be successfully deleted
    // It verifies that the user is removed from the database
    [Fact]
    public async Task DeleteUser_ExistingUser_RemovesUserFromDatabase()
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
        await userService.DeleteUser(user.Id);

        // Assert
        var dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.Null(dbUser);
    }

    // Functional test
    // This tests that deleting a non-existing user throws an exception
    // It verifies correct error handling for invalid user IDs
    [Fact]
    public async Task DeleteUser_NonExistingUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var randomId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.DeleteUser(randomId)
        );
    }

    /// -----------------
    /// UploadProfilePicture tests
    /// -----------------

    // Functional test
    // This tests that a profile picture can be successfully uploaded
    // It verifies that the user gets a profile picture assigned and stored in the database
    [Fact]
    public async Task UploadProfilePicture_ValidFile_UpdatesUserProfilePicture()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var user = TestUserFactory.Create();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new UploadProfilePictureDto
        {
            FileName = "profile.png",
            AltText = "User profile picture",
            File = TestFileFactory.CreateFormFile("image/png")
        };

        // Act
        var result = await userService.UploadProfilePicture(user.Id, dto);

        // Assert
        Assert.NotNull(result);

        var updatedUser = await dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        Assert.NotNull(updatedUser.ProfilePicture);
        Assert.Equal("profile.png", updatedUser.ProfilePicture.FileName);
    }

    // Functional test
    // This tests that uploading a profile picture replaces an existing one
    // It verifies that the old profile picture is removed and replaced correctly
    [Fact]
    public async Task UploadProfilePicture_ExistingProfilePicture_ReplacesOldPicture()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var oldFile = new StudyFile
        {
            Id = Guid.NewGuid(),
            FileName = "old.png",
            BlobName = "old_blob",
            ContentType = "image/png"
        };

        var user = TestUserFactory.Create();
        user.ProfilePicture = oldFile;

        dbContext.Users.Add(user);
        dbContext.Files.Add(oldFile);
        await dbContext.SaveChangesAsync();

        var dto = new UploadProfilePictureDto
        {
            FileName = "new.png",
            AltText = "Updated profile picture",
            File = TestFileFactory.CreateFormFile("image/png")
        };

        // Act
        var result = await userService.UploadProfilePicture(user.Id, dto);

        // Assert
        var updatedUser = await dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        Assert.NotNull(updatedUser?.ProfilePicture);
        Assert.Equal("new.png", updatedUser.ProfilePicture.FileName);
        Assert.NotEqual("old.png", updatedUser.ProfilePicture.FileName);
    }

    // Functional test
    // This tests that uploading a profile picture for a non-existing user throws an exception
    // It verifies correct error handling for invalid user IDs
    [Fact]
    public async Task UploadProfilePicture_InvalidUser_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var dto = new UploadProfilePictureDto
        {
            FileName = "profile.png",
            File = TestFileFactory.CreateFormFile("image/png")
        };

        var randomId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.UploadProfilePicture(randomId, dto)
        );
    }
    
    // Functional test
    // This tests that when fileName is empty, a fallback generated name is used
    // It verifies that the service generates a safe default file name
    [Fact]
    public async Task UploadProfilePicture_FileNameEmpty_GeneratesFallbackFileName()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var user = TestUserFactory.Create();
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new UploadProfilePictureDto
        {
            FileName = "",
            File = TestFileFactory.CreateFormFile("image/png", "original.png")
        };

        // Act
        var result = await userService.UploadProfilePicture(user.Id, dto);

        // Assert
        var updatedUser = await dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        Assert.NotNull(updatedUser.ProfilePicture);

        Assert.False(string.IsNullOrWhiteSpace(updatedUser.ProfilePicture.FileName));
        Assert.NotEqual("", updatedUser.ProfilePicture.FileName);
    }
    
    /// -----------------
    /// ChangePassword tests
    /// -----------------
    
    // Functional test
    // This tests that a user can successfully change their password
    // It verifies that the password hash is updated in the database
    [Fact]
    public async Task ChangePassword_ValidCurrentPassword_UpdatesPassword()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var oldPassword = "OldPassword123!";
        var newPassword = "NewPassword123!";

        var user = TestUserFactory.Create();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(oldPassword);

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new ChangePasswordDto
        {
            CurrentPassword = oldPassword,
            NewPassword = newPassword
        };

        // Act
        await userService.ChangePassword(user.Id, dto);

        // Assert
        var updatedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        Assert.NotNull(updatedUser);
        Assert.True(BCrypt.Net.BCrypt.Verify(newPassword, updatedUser.PasswordHash));
    }

    // Functional test
    // This tests that providing an incorrect current password throws an exception
    // It verifies that password cannot be changed without valid authentication
    [Fact]
    public async Task ChangePassword_InvalidCurrentPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        var user = TestUserFactory.Create();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new ChangePasswordDto
        {
            CurrentPassword = "WrongPassword",
            NewPassword = "NewPassword123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => userService.ChangePassword(user.Id, dto)
        );
    }

    // Functional test
    // This tests that setting the same password as current is not allowed
    // It verifies that redundant password updates are rejected
    [Fact]
    public async Task ChangePassword_SameAsCurrentPassword_ThrowsArgumentException()
    {
        // Arrange
        var dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        var blobService = new FakeBlobService();
        var userService = new UserService(dbContext, mapper, blobService);

        const string password = "SamePassword123!";

        var user = TestUserFactory.Create(passwordHash: PasswordHelper.HashPassword(password));

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var dto = new ChangePasswordDto
        {
            CurrentPassword = password,
            NewPassword = password
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => userService.ChangePassword(user.Id, dto)
        );
    }
    
}