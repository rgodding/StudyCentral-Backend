using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Auth;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> Register(RegisterDto request);
    Task<AuthResponseDto> Login(LoginDto request);
}

public class AuthService : IAuthService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;


    public AuthService(StudyDbContext dbContext, IJwtService jwtService, IMapper mapper)
    {
        _dbContext = dbContext;
        _jwtService = jwtService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Register(RegisterDto dto)
    {
        var emailExists = await _dbContext.Users
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
            throw new ExceptionMiddleware.ConflictException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = UserRole.Student,
            PasswordHash = PasswordHelper.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var userDto = _mapper.Map<UserDto>(user);

        var token = _jwtService.GenerateToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            User = userDto
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
            Token = _jwtService.GenerateToken(_mapper.Map<UserDto>(user)),
            User = _mapper.Map<UserDto>(user)
        };
    }
}