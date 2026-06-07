using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.Account;
using StudyCentral.API.Models.DTOs.Admin.User;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IUserService
{
    // CRUD
    Task<List<UserDto>> GetAll();
    Task<UserDto> GetById(Guid userId);
    Task<UserDto> Create(CreateUserDto dto);
    Task<UserDto> Update(Guid userId, UpdateUserDto dto);
    Task Delete(Guid userId);

    // Admin User Management
    Task<UserDto> AdminUpdateUser(Guid userId, AdminUpdateUserDto dto);
    Task AdminUpdatePassword(Guid userId, AdminUpdateUserPasswordDto dto);

    // User Management
    Task<UserDto> GetMe(Guid userId);
    Task<UserDto> UpdateMe(Guid userId, UpdateMeRequest request);
    Task ChangePassword(Guid userId, ChangePasswordRequest request);

    // File Management
    Task AddProfilePicture(Guid userId, IFormFile file);
    Task DeleteProfilePicture(Guid userId);
}

public class UserService : IUserService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IStudyFileService _studyFileService;

    public UserService(StudyDbContext dbContext, IMapper mapper, IStudyFileService studyFileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _studyFileService = studyFileService;
    }

    // --------------
    //  CRUD METHODS
    // --------------

    public async Task<List<UserDto>> GetAll()
    {
        var users = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .ToListAsync();

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> GetById(Guid userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> Create(CreateUserDto dto)
    {
        var emailExists = await _dbContext.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
            throw new InvalidOperationException("User with this email already exists");

        var newUser = _mapper.Map<User>(dto);
        newUser.PasswordHash = PasswordHelper.HashPassword(dto.Password);

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<UserDto>(newUser);
    }

    public async Task<UserDto> Update(Guid userId, UpdateUserDto dto)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        // Check email uniqueness
        if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
        {
            // Check if email is already taken
            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == dto.Email && u.Id != userId);

            if (emailExists)
                throw new InvalidOperationException("Email already exists");
        }

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.Email = dto.Email ?? user.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task Delete(Guid userId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    // --------------
    //  ADMIN USER MANAGEMENT METHODS
    // --------------
    public async Task<UserDto> AdminUpdateUser(Guid userId, AdminUpdateUserDto dto)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        // Check email uniqueness
        if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
        {
            // Check if email is already taken
            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == dto.Email && u.Id != userId);

            if (emailExists)
                throw new InvalidOperationException("Email already exists");
        }

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.Email = dto.Email ?? user.Email;
        user.Role = dto.Role ?? user.Role;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task AdminUpdatePassword(Guid userId, AdminUpdateUserPasswordDto dto)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    // --------------
    //  USER MANAGEMENT METHODS
    // --------------
    public async Task<UserDto> GetMe(Guid userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateMe(Guid userId, UpdateMeRequest request)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Email = request.Email ?? user.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task ChangePassword(
        Guid userId,
        ChangePasswordRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (!PasswordHelper.VerifyHash(
                request.CurrentPassword,
                user.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Current password is incorrect");
        }

        user.PasswordHash =
            PasswordHelper.HashPassword(request.NewPassword);

        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    // --------------
    //  FILE METHODS
    // --------------
    public async Task AddProfilePicture(Guid userId, IFormFile file)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        // Remove existing profile picture
        if (user.ProfilePicture != null)
        {
            await _studyFileService.DeleteFile(user.ProfilePicture.Id);

            user.ProfilePicture = null;
            user.ProfilePictureId = null;
        }

        var profilePicture = await _studyFileService.UploadFile(file, userId);

        user.ProfilePicture = profilePicture;
        user.ProfilePictureId = profilePicture.Id;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProfilePicture(Guid userId)
    {
        var user = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (user.ProfilePicture == null)
            throw new InvalidOperationException("User does not have a profile picture");

        var profilePictureId = user.ProfilePicture.Id;

        user.ProfilePicture = null;
        user.ProfilePictureId = null;

        await _dbContext.SaveChangesAsync();

        await _studyFileService.DeleteFile(profilePictureId);
    }
}