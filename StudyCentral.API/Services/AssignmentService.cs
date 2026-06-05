using System.Security;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAssignmentService
{
    // Core CRUD
    Task<List<AssignmentPreviewDto>> GetAll();
    Task<AssignmentDto> GetById(Guid assignmentId);
    Task<AssignmentDto> Create(Guid teacherId, CreateAssignmentDto dto);
    Task<AssignmentDto> Update(Guid teacherId, Guid assignmentId, UpdateAssignmentDto dto);
    Task Delete(Guid teacherId, Guid assignmentId);

    // Course-based access
    Task<List<AssignmentPreviewDto>> GetByCourseId(Guid courseId);
    Task<List<AssignmentPreviewDto>> GetByStudentId(Guid studentId);

    // Utilities
    Task<List<AssignmentPreviewDto>> GetUpcoming(Guid courseId);
    Task<List<AssignmentPreviewDto>> GetOverdue(Guid courseId);

    // Files
    Task<int> GetFileCount(Guid assignmentId);
    Task AddFile(Guid teacherId, Guid assignmentId, Guid fileId);
    Task RemoveFile(Guid teacherId, Guid assignmentId, Guid fileId);

    // Internal validation
    Task<bool> IsTeacherOwner(Guid teacherId, Guid assignmentId);
}

public class AssignmentService : IAssignmentService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public AssignmentService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<AssignmentPreviewDto>> GetAll()
    {
        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.Files)
            .ToListAsync();
        
        return _mapper.Map<List<AssignmentPreviewDto>>(assignments);
    }

    public async Task<AssignmentDto> GetById(Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.Files)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
        
        if(assignment == null)
            throw new KeyNotFoundException("Assignment not found");
        
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> Create(Guid teacherId, CreateAssignmentDto dto)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == dto.CourseId);
        
        if(course == null)
            throw new KeyNotFoundException("Course not found");
        
        if(course.TeacherId != teacherId)
            throw new SecurityException("Teacher not authorized to create assignments for this course");
        
        var exists = await _dbContext.Assignments
            .AnyAsync(a => a.Title == dto.Title && a.CourseId == dto.CourseId);
        
        if(exists)
            throw new ExceptionMiddleware.ConflictException("Assignment with the same title already exists in this course");
        
        var assignment = _mapper.Map<Assignment>(dto);
        assignment.CourseId = dto.CourseId;
        
        await _dbContext.Assignments.AddAsync(assignment);
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> Update(Guid teacherId, Guid assignmentId, UpdateAssignmentDto dto)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
        
        if(assignment == null)
            throw new KeyNotFoundException("Assignment not found");
        
        if(assignment.Course.TeacherId != teacherId)
            throw new SecurityException("Teacher not authorized to update this assignment");

        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            var titleExists = await _dbContext.Assignments
                .AnyAsync(a =>
                    a.Title == dto.Title &&
                    a.Id != assignmentId &&
                    a.CourseId == assignment.CourseId);

            if (titleExists)
                throw new ExceptionMiddleware.ConflictException(
                    "Assignment with the same title already exists in this course");
        }
        
        assignment.Title = dto.Title ?? assignment.Title;
        assignment.Description = dto.Description ?? assignment.Description;
        assignment.Deadline = dto.Deadline ?? assignment.Deadline;
        assignment.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<AssignmentDto>(assignment);       
    }

    public async Task Delete(Guid teacherId, Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
        
        if(assignment == null)
            throw new KeyNotFoundException("Assignment not found");
        
        if(assignment.Course.TeacherId != teacherId)
            throw new SecurityException("Teacher not authorized to delete this assignment");
        
        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<AssignmentPreviewDto>> GetByCourseId(Guid courseId)
    {
        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.Files)
            .Where(a => a.CourseId == courseId)
            .ToListAsync();
        
        return _mapper.Map<List<AssignmentPreviewDto>>(assignments);       
    }

    public async Task<List<AssignmentPreviewDto>> GetByStudentId(Guid studentId)
    {
        var studentExists = await _dbContext.Users
            .AnyAsync(u => u.Id == studentId);

        if (!studentExists)
            throw new KeyNotFoundException("Student not found");

        var assignments = await _dbContext.Submissions
            .Where(s => s.StudentId == studentId)
            .Select(s => s.Assignment)
            .Distinct()
            .Include(a => a.Course)
            .Include(a => a.Files)
            .ToListAsync();

        return _mapper.Map<List<AssignmentPreviewDto>>(assignments);
    }

    public async Task<List<AssignmentPreviewDto>> GetUpcoming(Guid courseId)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
            throw new KeyNotFoundException("Course not found");
        
        var currentDate = DateTime.UtcNow;
        
        var assignments = await _dbContext.Assignments
            .Where(a => a.CourseId == courseId && a.Deadline != null && a.Deadline > currentDate)
            .OrderBy(a => a.Deadline)
            .ToListAsync();
        
        return _mapper.Map<List<AssignmentPreviewDto>>(assignments);      
    }

    public async Task<List<AssignmentPreviewDto>> GetOverdue(Guid courseId)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
            throw new KeyNotFoundException("Course not found");

        var currentDate = DateTime.UtcNow;

        var assignments = await _dbContext.Assignments
            .Where(a =>
                a.CourseId == courseId &&
                a.Deadline != null &&
                a.Deadline < currentDate)
            .OrderByDescending(a => a.Deadline)
            .ToListAsync();

        return _mapper.Map<List<AssignmentPreviewDto>>(assignments);
    }

    public async Task<int> GetFileCount(Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Files)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
        
        if(assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        return assignment.Files.Count;
    }

    public Task AddFile(Guid teacherId, Guid assignmentId, Guid fileId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFile(Guid teacherId, Guid assignmentId, Guid fileId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsTeacherOwner(Guid teacherId, Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        return assignment.Course.TeacherId == teacherId;
    }
}