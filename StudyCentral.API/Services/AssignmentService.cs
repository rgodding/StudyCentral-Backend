using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IAssignmentService
{
    // CRUD
    Task<List<AssignmentDto>> GetAll();
    Task<AssignmentDto> GetById(Guid assignmentId);
    Task<AssignmentDto> Create(CreateAssignmentDto dto);
    Task<AssignmentDto> Update(Guid assignmentId, UpdateAssignmentDto dto);
    Task Delete(Guid assignmentId);
    
    // Teacher Methods
    Task<List<AssignmentDto>> GetAssignmentsByTeacherId(
        Guid teacherId);

    Task<AssignmentDto> GetAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId);

    Task<AssignmentDto> CreateAssignmentByTeacherId(
        Guid teacherId,
        CreateAssignmentDto dto);

    Task<AssignmentDto> UpdateAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId,
        UpdateAssignmentDto dto);

    Task DeleteAssignmentByTeacherId(
        Guid teacherId,
        Guid assignmentId);
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
    
    // --------------
    //  CRUD METHODS
    // --------------
    
    public async Task<List<AssignmentDto>> GetAll()
    {
        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .Include(a => a.StudyFiles)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }
    
    public async Task<AssignmentDto> GetById(Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        return _mapper.Map<AssignmentDto>(assignment);
    }
    
    public async Task<AssignmentDto> Create(CreateAssignmentDto dto)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == dto.CourseId);

        if (!courseExists)
            throw new KeyNotFoundException("Course not found");

        var exists = await _dbContext.Assignments
            .AnyAsync(a => a.Name == dto.Name && a.CourseId == dto.CourseId);

        if (exists)
            throw new InvalidOperationException("Assignment with this name already exists in this course");

        var assignment = _mapper.Map<Assignment>(dto);
        assignment.Id = Guid.NewGuid();
        assignment.CreatedAt = DateTime.UtcNow;
        assignment.UpdatedAt = DateTime.UtcNow;

        _dbContext.Assignments.Add(assignment);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AssignmentDto>(assignment);
    }
    
    public async Task<AssignmentDto> Update(Guid assignmentId, UpdateAssignmentDto dto)
    {
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != assignment.Name)
        {
            var exists = await _dbContext.Assignments
                .AnyAsync(a =>
                    a.Name == dto.Name &&
                    a.CourseId == assignment.CourseId &&
                    a.Id != assignmentId);

            if (exists)
                throw new InvalidOperationException("Assignment with this Name already exists in this course");

            assignment.Name = dto.Name;
        }

        assignment.Description = dto.Description ?? assignment.Description;
        assignment.Deadline = dto.Deadline ?? assignment.Deadline;
        assignment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<AssignmentDto>(assignment);
    }
    
    public async Task Delete(Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");

        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }
    
    // --------------
    //  TEACHER METHODS
    // --------------

    public async Task<List<AssignmentDto>> GetAssignmentsByTeacherId(Guid teacherId)
    {
        var assignments = await _dbContext.Assignments
            .Where(a => a.Course.TeacherId == teacherId)
            .ToListAsync();

        return _mapper.Map<List<AssignmentDto>>(assignments);
    }
    
    public async Task<AssignmentDto> GetAssignmentByTeacherId(Guid teacherId, Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a =>
                a.Id == assignmentId &&
                a.Course.TeacherId == teacherId);
        
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> CreateAssignmentByTeacherId(Guid teacherId, CreateAssignmentDto dto)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == dto.CourseId && c.TeacherId == teacherId);
        
        if (!courseExists)
            throw new KeyNotFoundException("Course not found");
        
        var assignment = _mapper.Map<Assignment>(dto);
        
        _dbContext.Assignments.Add(assignment);
        await _dbContext.SaveChangesAsync();
        
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task<AssignmentDto> UpdateAssignmentByTeacherId(Guid teacherId, Guid assignmentId, UpdateAssignmentDto dto)
    {
        var assignment = await _dbContext.Assignments
            .Include(a => a.Course)
            .FirstOrDefaultAsync(a =>
                a.Id == assignmentId &&
                a.Course.TeacherId == teacherId);
        
        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");
        
        assignment.Name = dto.Name ?? assignment.Name;
        assignment.Description = dto.Description ?? assignment.Description;
        assignment.Deadline = dto.Deadline ?? assignment.Deadline;
        assignment.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<AssignmentDto>(assignment);
    }

    public async Task DeleteAssignmentByTeacherId(Guid teacherId, Guid assignmentId)
    {
        var assignment = await _dbContext.Assignments
            .FirstOrDefaultAsync(a =>
                a.Id == assignmentId &&
                a.Course.TeacherId == teacherId);
        
        if (assignment == null)
            throw new KeyNotFoundException("Assignment not found");
        
        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }
}