using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ICourseService
{
    Task<List<Course>> GetAllCourses();
    Task<Course> GetCourse(Guid id);
    
    Task<Course> CreateCourse(CreateCourseDto request);
    Task<Course> UpdateCourse(Guid id, UpdateCourseDto request);
    Task DeleteCourse(Guid id);
    
    Task<List<Course>> GetCoursesByTeacherId(Guid teacherId);
    Task<List<Course>> GetCoursesByStudentId(Guid studentId);
    
    Task<List<User>> GetEnrolledStudents(Guid courseId);
    
    Task EnrollStudent(Guid courseId, Guid studentId);
    Task UnenrollStudent(Guid courseId, Guid studentId);
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

    public async Task<List<Course>> GetAllCourses()
    {
        var result = await _dbContext.Courses
            .Include(c => c.Teacher)
            .ToListAsync();
        return result;
    }

    public async Task<Course> GetCourse(Guid id)
    {
        var result = await  _dbContext.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("Course not found");
        return result;
    }

    public async Task<Course> CreateCourse(CreateCourseDto request)
    {
        // Check if duplicate title
        var existingCourse = await  _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Title == request.Title);

        if (existingCourse != null)
        {
            throw new ExceptionMiddleware.ConflictException("Course with the same title already exists");
        }
        
        var  course = _mapper.Map<Course>(request);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
        return course;
    }

    public async Task<Course> UpdateCourse(Guid id, UpdateCourseDto request)
    {
        var existingCourse = await  _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("Course not found");
        
        existingCourse.Title = request.Title ?? existingCourse.Title;
        existingCourse.Description = request.Description ?? existingCourse.Description;
        existingCourse.TeacherId = request.TeacherId ?? existingCourse.TeacherId;
        
        await _dbContext.SaveChangesAsync();
        return existingCourse;
    }

    public async Task DeleteCourse(Guid id)
    {
        var existingCourse = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("Course not found");
        
        _dbContext.Courses.Remove(existingCourse);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Course>> GetCoursesByTeacherId(Guid teacherId)
    {
        var result = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();
        return result;
    }

    public async Task<List<Course>> GetCoursesByStudentId(Guid studentId)
    {
        var result = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Where(c => c.TeacherId == studentId)
            .ToListAsync();
        return result;
    }

    public async Task<List<User>> GetEnrolledStudents(Guid courseId)
    {
        var result = await  _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Students)
            .Where(c => c.Id == courseId)
            .ToListAsync();
        
        return result.SelectMany(c => c.Students).ToList();
    }

    public async Task EnrollStudent(Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new KeyNotFoundException("Course not found");
        
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == studentId)
            ?? throw new KeyNotFoundException("Student not found");
        
        course.Students.Add(student);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UnenrollStudent(Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new KeyNotFoundException("Course not found");
        
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == studentId)
            ?? throw new KeyNotFoundException("Student not found");
        
        course.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
    }
}