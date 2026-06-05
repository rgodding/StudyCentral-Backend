using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ICourseService
{
    // CRUD
    Task<List<CourseDto>> GetAll();
    Task<CourseDto> GetById(Guid courseId);
    Task<CourseDto> Create(CreateCourseDto dto);
    Task<CourseDto> Update(Guid courseId, UpdateCourseDto dto);
    Task Delete(Guid courseId);
}

public class CourseService : ICourseService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public CourseService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// --------------
    ///  CRUD METHODS
    /// --------------
    
    public async Task<List<CourseDto>> GetAll()
    {
        var courses = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .ToListAsync();

        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<CourseDto> GetById(Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> Create(CreateCourseDto dto)
    {
        var course = _mapper.Map<Course>(dto);
        course.Id = Guid.NewGuid();

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> Update(Guid courseId, UpdateCourseDto dto)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        course.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task Delete(Guid courseId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync();
    }
}