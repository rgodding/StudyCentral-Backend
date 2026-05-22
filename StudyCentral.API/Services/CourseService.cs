using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.CourseModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ICourseService
{
    public Task<List<Course>> GetCourses();
    public Task<Course> GetCourseById(Guid id);
    public Task<Course> CreateCourse(CreateCourseRequestModel request);
    public Task<Course> UpdateCourse(Guid id, UpdateCourseRequestModel request);
    public Task DeleteCourse(Guid id);
    
    public Task AddStudentToCourse(Guid courseId, Guid studentId);
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

    public async Task<List<Course>> GetCourses()
    {
        return await _dbContext.Courses.ToListAsync();
    }

    public async Task<Course> GetCourseById(Guid id)
    {
        return await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id)
               ?? throw new KeyNotFoundException($"Course with ID {id} not found.");
    }

    public async Task<Course> CreateCourse(CreateCourseRequestModel request)
    {
        var course = _mapper.Map<Course>(request);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
        return course;
    }

    public async Task<Course> UpdateCourse(Guid id, UpdateCourseRequestModel request)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCourse(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task AddStudentToCourse(Guid courseId, Guid studentId)
    {
        // Get the student and course
        var student = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == studentId);
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
        
        // Add the student to the course
        student.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
    }
}