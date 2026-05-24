using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Auth;
using StudyCentral.API.Models.Dtos.Users;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> SignUp(SignUpDto signUpDto);
    Task<AuthResponseDto> SignIn(SignInDto signInDto);
    Task<UserDto> GetCurrentUser(Guid userId);
}

public class AuthService : IAuthService
{
    private readonly StudyDbContext _dbContext;
    private readonly JwtHelper _jwtHelper;

    public AuthService(StudyDbContext dbContext, JwtHelper jwtHelper)
    {
        _dbContext = dbContext;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponseDto> SignUp(SignUpDto signUpDto)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == signUpDto.Email);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists");
        }

        var user = new User
        {
            Email = signUpDto.Email,
            PasswordHash = PasswordHelper.HashPassword(signUpDto.Password),
            FirstName = signUpDto.FirstName,
            LastName = signUpDto.LastName,
            Role = UserRole.User
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return new AuthResponseDto
        {
            Token = _jwtHelper.GenerateToken(user),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString()
            }
        };
    }

    public async Task<AuthResponseDto> SignIn(SignInDto signInDto)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == signInDto.Email);

        if (user == null)
        {
            throw new Exception("Invalid credentials");
        }

        var validPassword = PasswordHelper.VerifyHash(signInDto.Password, user.PasswordHash);
        if (!validPassword)
        {
            throw new Exception("Invalid credentials");
        }

        var token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString()
            }
        };
    }

    public async Task<UserDto> GetCurrentUser(Guid userId)
    {
        var user = await _dbContext.Users
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
            Role = user.Role.ToString()
        };
    }
}