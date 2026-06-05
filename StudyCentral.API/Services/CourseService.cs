using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;
using StudyCentral.API.Services;

namespace StudyCentral.API.Services

{
    public interface ICourseService
    {
        // Core CRUD
        Task<List<CoursePreviewDto>> GetAllCourses();
        Task<CourseDto> GetCourseById(Guid courseId);
        Task<CourseDto> CreateCourse(Guid teacherId, CreateCourseDto dto);
        Task<CourseDto> UpdateCourse(Guid courseId, UpdateCourseDto dto);
        Task DeleteCourse(Guid courseId);

        // General Functions
        Task<List<CoursePreviewDto>> GetCoursesByUserId(Guid userId);
        Task<List<CoursePreviewDto>> GetCoursesByTeacherId(Guid teacherId);
        Task<List<CoursePreviewDto>> GetCoursesByStudentId(Guid studentId);

        // Teacher Functions
        Task AddStudentToCourse(Guid teacherId, Guid courseId, Guid studentId);
        Task RemoveStudentFromCourse(Guid teacherId, Guid courseId, Guid studentId);
        Task<bool> IsStudentEnrolled(Guid courseId, Guid studentId);

        // Utilities
        Task ClearCourseStudents(Guid courseId);
    }
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

    // Core CRUD
    public async Task<List<CoursePreviewDto>> GetAllCourses()
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.CourseStudents)
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    public async Task<CourseDto> GetCourseById(Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateCourse(Guid teacherId, CreateCourseDto dto)
    {
        var exists = await _dbContext.Courses
            .AnyAsync(c => c.Title == dto.Title);

        if (exists)
            throw new InvalidOperationException("Course with the same title already exists");

        var teacher = await _dbContext.Users.FindAsync(teacherId);

        if (teacher == null)
            throw new KeyNotFoundException("Teacher not found");

        if (teacher.Role != UserRole.Teacher)
            throw new InvalidOperationException("Only teachers can be assigned to courses");

        var course = _mapper.Map<Course>(dto);
        course.TeacherId = teacher.Id;

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> UpdateCourse(Guid courseId, UpdateCourseDto dto)
    {
        var titleExists = await _dbContext.Courses
            .AnyAsync(c => c.Title == dto.Title && c.Id != courseId);

        if (titleExists)
            throw new InvalidOperationException("Course with the same title already exists");

        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (!string.IsNullOrWhiteSpace(dto.Title))
            course.Title = dto.Title;

        if (!string.IsNullOrWhiteSpace(dto.Description))
            course.Description = dto.Description;

        course.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task DeleteCourse(Guid courseId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        _dbContext.Courses.Remove(course);
        await _dbContext.SaveChangesAsync();
    }

    // General Functions
    public async Task<List<CoursePreviewDto>> GetCoursesByUserId(Guid userId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.CourseStudents)
            .Where(c =>
                c.TeacherId == userId ||
                c.CourseStudents.Any(cs => cs.StudentId == userId))
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    public async Task<List<CoursePreviewDto>> GetCoursesByTeacherId(Guid teacherId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.CourseStudents)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    public async Task<List<CoursePreviewDto>> GetCoursesByStudentId(Guid studentId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Include(c => c.CourseStudents)
            .Where(c => c.CourseStudents.Any(s => s.StudentId == studentId))
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    // Teacher Functions
    public async Task AddStudentToCourse(Guid teacherId, Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.TeacherId != teacherId)
            throw new UnauthorizedAccessException("Teacher does not have permission");

        var studentExists = await _dbContext.Users
            .AnyAsync(u => u.Id == studentId);
        
        if (!studentExists)
            throw new KeyNotFoundException("Student not found");
        
        if(course.CourseStudents.Any(s => s.StudentId == studentId))
            throw new InvalidOperationException("Student already enrolled in course");

        course.CourseStudents.Add(new CourseStudent
        {
            CourseId = courseId,
            StudentId = studentId
        });
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveStudentFromCourse(Guid teacherId, Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.TeacherId != teacherId)
            throw new UnauthorizedAccessException("Teacher does not have permission");

        var student = course.CourseStudents
            .FirstOrDefault(s => s.StudentId == studentId);

        if (student == null)
            throw new InvalidOperationException("Student not enrolled in course");

        course.CourseStudents.Remove(student);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsStudentEnrolled(Guid courseId, Guid studentId)
    {
        return await _dbContext.Courses
            .AnyAsync(c => c.Id == courseId &&
                           c.CourseStudents.Any(s => s.StudentId == studentId));
    }

    public async Task ClearCourseStudents(Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        course.CourseStudents.Clear();

        await _dbContext.SaveChangesAsync();
    }
}