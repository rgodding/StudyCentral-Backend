using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Assignments;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAssignmentService
{
    Task<Assignment> GetAssignmentById(Guid assignmentId);
    Task<List<Assignment>> GetAssignmentsByCourseId(Guid courseId);
    
    Task<Assignment> CreateAssignment(Guid teacherId, Guid courseId, CreateAssignmentDto request);
    Task<Assignment> UpdateAssignment(
        Guid teacherId,
        Guid assignmentId,
        UpdateAssignmentDto request
        );
    Task DeleteAssignment(Guid teacherId, Guid assignmentId);
    
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

    public async Task<Assignment> GetAssignmentById(Guid assignmentId)
    {
        return await  _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId)
            ?? throw new KeyNotFoundException("Assignment not found");
    }

    public async Task<List<Assignment>> GetAssignmentsByCourseId(Guid courseId)
    {
        return await _dbContext.Assignments
            .Where(a => a.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Assignment> CreateAssignment(Guid teacherId, Guid courseId, CreateAssignmentDto request)
    {
        var teacher = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher)
            ?? throw new KeyNotFoundException("Teacher not found");
        
        var course = await  _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacher.Id)
            ?? throw new KeyNotFoundException("Course not found");
        
        var assignment = _mapper.Map<Assignment>(request);
        
        assignment.Course = course;
        _dbContext.Assignments.Add(assignment);
        await _dbContext.SaveChangesAsync();
        return assignment;
    }

    public async Task<Assignment> UpdateAssignment(Guid teacherId, Guid assignmentId, UpdateAssignmentDto request)
    {
        var teacher = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher)
            ?? throw new KeyNotFoundException("Teacher not found");
        
        var assignment = await  _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId && a.Course.TeacherId == teacher.Id)
            ?? throw new KeyNotFoundException("Course not found");
        
        assignment.Title = request.Title ??  assignment.Title;
        assignment.Description = request.Description ??  assignment.Description;
        assignment.Deadline = request.Deadline ??  assignment.Deadline;
        
        await _dbContext.SaveChangesAsync();
        return assignment;
    }

    public async Task DeleteAssignment(Guid teacherId, Guid assignmentId)
    {
        var teacher = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == teacherId && u.Role == UserRole.Teacher)
            ?? throw new KeyNotFoundException("Teacher not found");
        
        var assignment = await  _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId && a.Course.TeacherId == teacher.Id)
            ?? throw new KeyNotFoundException("Course not found");
        
        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }
}