using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.StudentModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IStudentService    
{
    // Profile
    public Task UploadProfilePicture(Guid userId, IFormFile file, string altText);
    
    // Courses
    public Task<List<Course>> GetCoursesByStudentId(Guid userId);
    public Task<Course> GetCourseById(Guid id);
    
    // Assignments
    Task<List<Assignment>> GetAssignmentsByStudentId(Guid userId);
    Task<Assignment> GetAssignmentById(Guid id);
    
    // Submissions
    Task CreateSubmission(
        Guid assignmentId,
        Guid studentId,
        CreateSubmissionRequestModel request
    );
    Task SubmitAssignment(
        Guid assignmentId, 
        Guid studentId,
        SubmitAssignmentRequestModel request);
    Task<List<Submission>> GetSubmissionsByStudentId(Guid studentId);
    Task<Submission> GetSubmissionById(Guid id);
    
    
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

    public Task<Course> GetCourseById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Assignment>> GetAssignmentsByStudentId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Assignment> GetAssignmentById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task CreateSubmission(Guid assignmentId, Guid studentId, CreateSubmissionRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task SubmitAssignment(Guid assignmentId, Guid studentId, SubmitAssignmentRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task<List<Submission>> GetSubmissionsByStudentId(Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task<Submission> GetSubmissionById(Guid id)
    {
        throw new NotImplementedException();
    }
}