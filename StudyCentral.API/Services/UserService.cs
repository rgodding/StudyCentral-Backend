using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Authentication;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.AuthModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IUserService
{
    Task<List<User>> GetUsers();
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByEmail(string email);
    Task<User> CreateUser(SignUpRequestModel request);
    Task<User> GetUserInfo(Guid id);
}

public class UserService : IUserService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;


    public UserService(StudyDbContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<User>> GetUsers()
    {
        var users = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .Include(u => u.Courses)
            .Include(u => u.Assignments)
            .Include(u => u.Submissions)
            .ToListAsync();
        return users;
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _dbContext.Users
                   .Include(u => u.ProfilePicture)
                   .FirstOrDefaultAsync(u => u.Id == id) 
               ?? throw new KeyNotFoundException("User with ID " + id + " not found.");
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _dbContext.Users
                   .FirstOrDefaultAsync(u => u.Email == email)
               ?? throw new KeyNotFoundException("User with email " + email + " not found.");
    }

    public async Task<User> CreateUser(SignUpRequestModel request)
    {
        Console.WriteLine("Creating user");
        var userExists = await _dbContext.Users.AnyAsync(x => x.Email == request.Email);
        if (userExists)
        {
            throw new ExceptionMiddleware.ConflictException($"Email already in use");
        }

        Console.WriteLine("Mapping request to user");
        var newUser = _mapper.Map<User>(request);

        var hashedPassword = PasswordHelper.HashPassword(request.Password);

        newUser.PasswordHash = hashedPassword;

        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();

        return newUser;
    }

    public async Task<User> GetUserInfo(Guid id)
    {
        return await _dbContext.Users
                   .Include(u => u.ProfilePicture)
                   .Include(u => u.Courses)
                   .FirstOrDefaultAsync(u => u.Id == id)
               ?? throw new KeyNotFoundException("User with ID " + id + " not found.");
    }
    
}
    