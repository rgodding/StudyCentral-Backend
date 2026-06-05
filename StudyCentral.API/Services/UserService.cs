using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
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
}

public class UserService : IUserService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// --------------
    ///  CRUD METHODS
    /// --------------
    
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
}