using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Register(RegisterDto dto);
    Task<AuthResponseDto> Login(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly StudyDbContext _dbContext;
    private readonly IJwtService _jwtService;


    public AuthService(StudyDbContext dbContext, IJwtService jwtService)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> Register(RegisterDto dto)
    {
        var emailExists = await _dbContext.Users.AnyAsync(u => u.Email == dto.Email);
        
        if (emailExists)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = UserRole.Student,
            PasswordHash = PasswordHelper.HashPassword(dto.Password)
        };
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token
        };
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");
        
        var validPassword = PasswordHelper.VerifyHash(dto.Password, user.PasswordHash);
        
        if(!validPassword)
            throw new UnauthorizedAccessException("Invalid credentials");

        return new AuthResponseDto
        {
            Token = _jwtService.GenerateToken(user)
        };
    }
}