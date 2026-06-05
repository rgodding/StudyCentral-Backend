using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IUserService
{
    // Core CRUD
    Task<List<UserPreviewDto>> GetAllUsers();
    Task<UserDto> GetUserById(Guid userId);
    Task<UserDto> CreateUser(CreateUserDto dto);
    Task<UserDto> UpdateUser(Guid userId, UpdateUserDto dto);
    Task DeleteUser(Guid userId);

    // Profile
    Task<UserDto> UploadProfilePicture(Guid userId, UploadProfilePictureDto dto);
    Task ChangePassword(Guid userId, ChangePasswordDto dto);

    // Admin
    Task<List<UserPreviewDto>> SearchUsers(string query);
    Task<List<UserPreviewDto>> GetUsersByRole(UserRole role);
    Task ChangeUserRole(Guid userId, UserRole role);
}

public class UserService : IUserService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IBlobService _blobService;

    public UserService(StudyDbContext dbContext, IMapper mapper, IBlobService blobService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<List<UserPreviewDto>> GetAllUsers()
    {
        var users = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .ToListAsync();

        return _mapper.Map<List<UserPreviewDto>>(users);
    }

    public async Task<UserDto> GetUserById(Guid userId)
    {
        var user = await _dbContext.Users
                       .Include(u => u.ProfilePicture)
                       .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new KeyNotFoundException("User with id not found");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUser(CreateUserDto dto)
    {
        var exists = await _dbContext.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (exists)
            throw new InvalidOperationException("Email already exists");

        var user = _mapper.Map<User>(dto);
        user.PasswordHash = PasswordHelper.HashPassword(dto.Password);
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUser(Guid userId, UpdateUserDto dto)
    {
        var user = await _dbContext.Users
                       .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new KeyNotFoundException("User not found");

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;

        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUser(Guid userId)
    {
        var user = await _dbContext.Users
                       .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new KeyNotFoundException("User not found");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserDto> UploadProfilePicture(Guid userId, UploadProfilePictureDto dto)
    {
        var user = await _dbContext.Users
                       .Include(u => u.ProfilePicture)
                       .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new KeyNotFoundException("User not found");

        // Delete old profile picture if it exists
        if (user.ProfilePicture != null)
        {
            await _blobService.DeleteFile(user.ProfilePicture.BlobName);
            _dbContext.Files.Remove(user.ProfilePicture);
        }
        
        // Generate filename
        var fileName = !string.IsNullOrWhiteSpace(dto.FileName)
            ? dto.FileName
            : $"{Guid.NewGuid()}_{dto.File.FileName}";
        
        // Upload new file
        var uploadResult = await _blobService.UploadFile(fileName, dto.File);
        
        // Create StudyFile entity
        var file = new StudyFile
        {
            FileName = uploadResult.FileName,
            BlobName = uploadResult.BlobName,
            ContentType = uploadResult.ContentType,
            Size = dto.File.Length,
            AltText = dto.AltText,
            UploadedById = user.Id,
        };
        
        // Assign Relationship
        user.ProfilePicture = file;
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }


    public async Task ChangePassword(Guid userId, ChangePasswordDto dto)
    {
        var user = await _dbContext.Users
                       .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new KeyNotFoundException("User not found");

        if (dto.NewPassword == dto.CurrentPassword)
            throw new ArgumentException("New password cannot be the same as current password");

        var isValidPassword = PasswordHelper.VerifyHash(dto.CurrentPassword, user.PasswordHash);

        if (!isValidPassword)
            throw new UnauthorizedAccessException("Invalid password");

        user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);

        await _dbContext.SaveChangesAsync();
    }

    public Task<List<UserPreviewDto>> SearchUsers(string query)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserPreviewDto>> GetUsersByRole(UserRole role)
    {
        var users = await _dbContext.Users
            .Where(u => u.Role == role)
            .Include(u => u.ProfilePicture)
            .ToListAsync();
        
        return _mapper.Map<List<UserPreviewDto>>(users);
    }

    public async Task ChangeUserRole(Guid userId, UserRole role)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new KeyNotFoundException("User not found");

        if (user.Role == role)
            throw new InvalidOperationException("User already has this role");
        
        user.Role = role;
        
        await _dbContext.SaveChangesAsync();
    }
}