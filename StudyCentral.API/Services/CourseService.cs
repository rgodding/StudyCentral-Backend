using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ICourseService
{
    Task<List<Course>> GetAllCourses();
    Task<Course> GetCourse(Guid id);
    
    Task<Course> CreateCourse(CreateCourseDto course);
    Task<Course> UpdateCourse(Guid id, UpdateCourseDto course);
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

    public CourseService(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Course>> GetAllCourses()
    {
        var result = await _dbContext.Courses
            .Include(c => c.Teacher)
            .ToListAsync();

        return result;
    }

    public Task<Course> GetCourse(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Course> CreateCourse(CreateCourseDto course)
    {
        throw new NotImplementedException();
    }

    public Task<Course> UpdateCourse(Guid id, UpdateCourseDto course)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCourse(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Course>> GetCoursesByTeacherId(Guid teacherId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Course>> GetCoursesByStudentId(Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetEnrolledStudents(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task EnrollStudent(Guid courseId, Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task UnenrollStudent(Guid courseId, Guid studentId)
    {
        throw new NotImplementedException();
    }
}