using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ICourseService
{
    Task<List<CourseDto>> GetAllCourses();
    Task<CourseDto> GetCourse(Guid id);
    
    Task<CourseDto> CreateCourse(CreateCourseDto courseDto);
    Task<CourseDto> UpdateCourse(Guid id, UpdateCourseDto courseDto);
    Task DeleteCourse(Guid id);
    
    Task<List<CourseDto>> GetCoursesByTeacherId(Guid teacherId);
    Task<List<CourseDto>> GetCoursesByStudentId(Guid studentId);
    
    Task<List<User>> GetEnrolledStudents(Guid courseId);
    
    Task EnrollStudent(Guid courseId, Guid studentId);
    Task UnenrollStudent(Guid courseId, Guid studentId);
}

public class CourseService : ICourseService
{
    public Task<List<CourseDto>> GetAllCourses()
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto> GetCourse(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto> CreateCourse(CreateCourseDto courseDto)
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto> UpdateCourse(Guid id, UpdateCourseDto courseDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCourse(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<CourseDto>> GetCoursesByTeacherId(Guid teacherId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CourseDto>> GetCoursesByStudentId(Guid studentId)
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