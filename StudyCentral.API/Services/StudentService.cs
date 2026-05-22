using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IStudentService    
{
    public Task UploadProfilePicture(Guid userId, IFormFile file, string altText);
    public Task<List<Course>> GetCoursesByStudentId(Guid userId);
}

public class StudentService : IStudentService
{
    private readonly IBlobService _blobService;
    private readonly IUserService _userService;
    private readonly StudyDbContext _dbContext;


    public StudentService(IBlobService blobService, IUserService userService, StudyDbContext dbContext)
    {
        _blobService = blobService;
        _userService = userService;
        _dbContext = dbContext;
    }

    public async Task UploadProfilePicture(Guid userId, IFormFile file, string altText)
    {
        // Check if the user exists
        var currentUser = await _dbContext.Users
            .Include(u => u.ProfilePicture)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        
        // Upload the new profile picture
        var newProfilePicture = new ImageFile
        {
            FileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}",
            AltText = altText,
            UploadedAt = DateTime.UtcNow,
        };
        
        // Upload the new profile picture to the blob storage
        await _blobService.UploadImage(newProfilePicture.FileName, file);
        
        // Delete the old profile picture if it exists
        if (currentUser.ProfilePicture != null)
        {
            await _blobService.DeleteImage(currentUser.ProfilePicture.FileName);
            _dbContext.Images.Remove(currentUser.ProfilePicture);
        }
        
        // Update the user's profile picture URL in the database
        currentUser.ProfilePicture = newProfilePicture;
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Course>> GetCoursesByStudentId(Guid userId)
    {
        var studentCourses = await _dbContext.Users
            .Include(u => u.Courses)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Courses)
            .ToListAsync();
        
        return studentCourses;
    }
}