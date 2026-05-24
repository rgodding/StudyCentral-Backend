using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Users;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IUserService
{
    Task<UserDto> GetCurrentUser(Guid userId);

    Task<UserDto> UpdateCurrentUser(
        Guid userId,
        UpdateUserDto dto);
    
    Task<UserDto> UpdateProfilePicture(Guid userId, IFormFile file, string? altText);
}

public class UserService : IUserService
{
    
    private readonly StudyDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UserService(StudyDbContext dbContext, IBlobService blobService)
    {
        _dbContext = dbContext;
        _blobService = blobService;
    }


    public async Task<UserDto> GetCurrentUser(Guid userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }
        

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            ProfilePictureUrl = user.ProfilePicture?.BlobName
        };
    }

    public async Task<UserDto> UpdateCurrentUser(
        Guid userId,
        UpdateUserDto dto)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;

        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
    }

    public async Task<UserDto> UpdateProfilePicture(Guid userId, IFormFile file, string? altText)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }
        
        if(!file.ContentType.StartsWith("image/"))
        {
            throw new Exception("File must be an image");
        }
        var blobName = $"{Guid.NewGuid()}_{file.FileName}";

        await _blobService.UploadFile(blobName, file);

        var studyFile = new StudyFile
        {
            Name = file.FileName,
            BlobName = blobName,
            ContentType = file.ContentType,
            Size = file.Length,
            Type = FileType.Image,
            AltText = altText,
            UploadedById = user.Id
        };
        
        if(user.ProfilePicture != null)
        {
            await _blobService.DeleteFile(user.ProfilePicture.BlobName);
            _dbContext.Files.Remove(user.ProfilePicture);
        }
        
        _dbContext.Files.Add(studyFile);
        user.ProfilePicture = studyFile;
        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
    }
}