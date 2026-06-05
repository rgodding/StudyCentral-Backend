using System.Security;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.Test.ServiceTests;

public interface ISubmissionService
{
    // Core CRUD
    Task<List<SubmissionPreviewDto>> GetAll();
    Task<SubmissionDto> GetById(Guid submissionId);
    Task<SubmissionDto> Create(Guid studentId, CreateSubmissionDto dto);
    Task<SubmissionDto> Update(Guid submissionId, UpdateSubmissionDto dto);
    Task Delete(Guid submissionId);

    // General Functions
    Task<List<SubmissionPreviewDto>> GetByAssignmentId(Guid assignmentId);
    Task<List<SubmissionPreviewDto>> GetByStudentId(Guid studentId);

    // Teacher operations 
    Task<SubmissionDto> Grade(Guid teacherId, Guid submissionId, GradeSubmissionDto dto);

    // File operations (domain actions)
    Task AddFile(Guid submissionId, Guid fileId);
    Task RemoveFile(Guid submissionId, Guid fileId);

    // Validation helpers (internal/business logic)
    Task<bool> SubmissionExists(Guid studentId, Guid assignmentId);
    Task<bool> IsStudentOwner(Guid studentId, Guid submissionId);
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

    public async Task<List<SubmissionPreviewDto>> GetAll()
    {
        var submissions = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.Files)
            .ToListAsync();

        return _mapper.Map<List<SubmissionPreviewDto>>(submissions);
    }

    public async Task<SubmissionDto> GetById(Guid submissionId)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.Files)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException("Submission not found");

        return _mapper.Map<SubmissionDto>(submission);
    }

    public async Task<SubmissionDto> Create(Guid studentId, CreateSubmissionDto dto)
    {
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == dto.AssignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        var student = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == studentId);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (student.Role != UserRole.Student)
            throw new SecurityException("Only students can create submissions");

        var isStudentInCourse = await _dbContext.CourseStudents
            .AnyAsync(cs =>
                cs.StudentId == studentId &&
                cs.CourseId == assignment.CourseId);

        if (!isStudentInCourse)
            throw new SecurityException("Student is not enrolled in the course for this assignment");

        var exists = await _dbContext.Submissions
            .AnyAsync(s => s.StudentId == studentId &&
                           s.AssignmentId == dto.AssignmentId);

        if (exists)
            throw new ExceptionMiddleware.ConflictException(
                "Submission already exists for this student and assignment");

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            AssignmentId = dto.AssignmentId,
            Comment = dto.Comment,
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Submissions.Add(submission);
        await _dbContext.SaveChangesAsync();

        var result = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.Files)
            .FirstAsync(s => s.Id == submission.Id);

        return _mapper.Map<SubmissionDto>(result);
    }

    public async Task<SubmissionDto> Update(Guid submissionId, UpdateSubmissionDto dto)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .Include(s => s.Files)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException("Submission not found");

        // business rule: cannot update graded submissions
        if (submission.Grade != null)
            throw new InvalidOperationException("Cannot update a graded submission");

        // update allowed fields only
        if (!string.IsNullOrWhiteSpace(dto.Comment))
            submission.Comment = dto.Comment;

        submission.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        var result = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Student)
            .Include(s => s.Files)
            .FirstAsync(s => s.Id == submission.Id);

        return _mapper.Map<SubmissionDto>(result);
    }

    public async Task Delete(Guid submissionId)
    {
        var submission = await _dbContext.Submissions
            .FirstOrDefaultAsync(s => s.Id == submissionId);

        if (submission == null)
            throw new KeyNotFoundException("Submission not found");

        // business rule: prevent deletion of graded submissions
        if (submission.Grade != null)
            throw new InvalidOperationException("Cannot delete a graded submission");

        _dbContext.Submissions.Remove(submission);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<SubmissionPreviewDto>> GetByAssignmentId(Guid assignmentId)
    {
        var submissions = await _dbContext.Submissions
            .Include(s => s.Student)
            .Include(s => s.Files)
            .Where(s => s.AssignmentId == assignmentId)
            .ToListAsync();

        return _mapper.Map<List<SubmissionPreviewDto>>(submissions);
    }

    public async Task<List<SubmissionPreviewDto>> GetByStudentId(Guid studentId)
    {
        var submissions = await _dbContext.Submissions
            .Include(s => s.Assignment)
            .Include(s => s.Files)
            .Where(s => s.StudentId == studentId)
            .ToListAsync();

        return _mapper.Map<List<SubmissionPreviewDto>>(submissions);
    }

    public async Task<SubmissionDto> Grade(Guid teacherId, Guid submissionId, GradeSubmissionDto dto)
    {
        var submission = await _dbContext.Submissions
            .Include(s => s.Assignment)
                .ThenInclude(a => a.Course)
            .Include(s => s.Student)
            .Include(s => s.Files)
            .FirstOrDefaultAsync(s => s.Id == submissionId);
        
        if (submission == null)
            throw new KeyNotFoundException("Submission not found");
        
        if (submission.Assignment?.Course?.TeacherId != teacherId)
            throw new SecurityException("Teacher not authorized to grade this submission");
        
        if(submission.Grade != null)
            throw new InvalidOperationException("Submission already graded");
        
        submission.Grade = dto.Grade;
        submission.Feedback = dto.Feedback;
        submission.GradedAt = DateTime.UtcNow;
        submission.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<SubmissionDto>(submission);       
    }

    public Task AddFile(Guid submissionId, Guid fileId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFile(Guid submissionId, Guid fileId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SubmissionExists(Guid studentId, Guid assignmentId)
    {
        return await _dbContext.Submissions
            .AnyAsync(s => s.StudentId == studentId && s.AssignmentId == assignmentId);
    }

    public async Task<bool> IsStudentOwner(Guid studentId, Guid submissionId)
    {
        return await _dbContext.Submissions
            .AnyAsync(s => s.Id == submissionId &&
                           s.StudentId == studentId);
    }
}