

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.Auth;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAuthService
{
    Task<AuthResponse> Register(RegisterRequest request);
    Task<AuthResponse> Login(LoginRequest request);
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

    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        var emailExists = await _dbContext.Users
            .AnyAsync(u => u.Email == request.Email);

        if (emailExists)
            throw new ExceptionMiddleware.ConflictException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = UserRole.Student,
            PasswordHash = PasswordHelper.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var userDto = _mapper.Map<UserDto>(user);

        var token = _jwtService.GenerateToken(userDto);

        return new AuthResponse
        {
            Token = token,
            User = userDto
        };
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");
        
        var validPassword = PasswordHelper.VerifyHash(request.Password, user.PasswordHash);
        
        if(!validPassword)
            throw new UnauthorizedAccessException("Invalid credentials");
        
        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(_mapper.Map<UserDto>(user)),
            User = _mapper.Map<UserDto>(user)
        };
    }
}