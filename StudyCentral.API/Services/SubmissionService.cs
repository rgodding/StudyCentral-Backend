using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Submissions;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ISubmissionService
{
    Task<Submission> GetSubmissionById(Guid submissionId);
    Task<List<Submission>> GetSubmissionsByCourseId(Guid courseId);
    Task<List<Submission>> GetSubmissionsByAssignmentId(Guid assignmentId);
    Task<List<Submission>> GetSubmissionsByStudentId(Guid studentId);
    
    Task<Submission> CreateSubmission(Guid studentId, Guid assignmentId, CreateSubmissionDto request);
    Task<Submission> UpdateSubmission(Guid studentId, Guid submissionId, UpdateSubmissionDto request);
    Task DeleteSubmission(Guid studentId, Guid submissionId);
}

public class SubmissionService : ISubmissionService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public SubmissionService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Submission> GetSubmissionById(Guid submissionId)
    {
        return await _dbContext.Submissions
            .FirstOrDefaultAsync(s => s.Id == submissionId)
            ?? throw new KeyNotFoundException("Submission not found");
    }

    public async Task<List<Submission>> GetSubmissionsByCourseId(Guid courseId)
    {
        return await _dbContext.Submissions
            .Where(s => s.Assignment.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<List<Submission>> GetSubmissionsByAssignmentId(Guid assignmentId)
    {
        return await  _dbContext.Submissions
            .Where(s => s.AssignmentId == assignmentId)
            .ToListAsync();
    }

    public async Task<List<Submission>> GetSubmissionsByStudentId(Guid studentId)
    {
        return await _dbContext.Submissions
            .Where(s => s.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<Submission> CreateSubmission(Guid studentId, Guid assignmentId, CreateSubmissionDto request)
    {
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new KeyNotFoundException("User not found");
        
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId)
            ?? throw new KeyNotFoundException("Assignment not found");
        
        var submission = _mapper.Map<Submission>(request);
        submission.Student = student;
        submission.Assignment = assignment;
        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();
        return submission;
    }

    public async Task<Submission> UpdateSubmission(Guid studentId, Guid submissionId, UpdateSubmissionDto request)
    {
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new KeyNotFoundException("User not found");
        
        var submission  = await _dbContext.Submissions
            .FirstOrDefaultAsync(s => s.Id == submissionId)
            ?? throw new KeyNotFoundException("Submission not found");
        
        submission.Comment = request.Comment ?? submission.Comment;
        submission.Feedback = request.Feedback ?? submission.Feedback;
        submission.Grade = request.Grade ?? submission.Grade;
        
        await _dbContext.SaveChangesAsync();
        return submission;
    }

    public async Task DeleteSubmission(Guid studentId, Guid submissionId)
    {
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new KeyNotFoundException("User not found");
        
        var submission = await _dbContext.Submissions
            .FirstOrDefaultAsync(s => s.Id == submissionId)
            ?? throw new KeyNotFoundException("Submission not found");
        _dbContext.Submissions.Remove(submission);
        await _dbContext.SaveChangesAsync();
    }
}