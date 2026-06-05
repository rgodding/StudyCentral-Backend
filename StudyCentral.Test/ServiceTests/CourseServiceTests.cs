using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Services;

public class CourseService : ICourseService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public CourseService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // -----------------
    // CORE CRUD
    // -----------------

    public async Task<List<CoursePreviewDto>> GetAllCourses()
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    public async Task<CourseDto> GetCourseById(Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.Teacher)
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

        var teacherExists = await _dbContext.Users
            .AnyAsync(u => u.Id == teacherId);

        if (!teacherExists)
            throw new KeyNotFoundException("Teacher not found");

        var course = _mapper.Map<Course>(dto);
        course.TeacherId = teacherId;

        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> UpdateCourse(Guid courseId, UpdateCourseDto dto)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            var titleExists = await _dbContext.Courses
                .AnyAsync(c => c.Title == dto.Title && c.Id != courseId);

            if (titleExists)
                throw new InvalidOperationException("Course with the same title already exists");

            course.Title = dto.Title;
        }

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

    // -----------------
    // GENERAL FUNCTIONS
    // -----------------

    public async Task<List<CoursePreviewDto>> GetCoursesByUserId(Guid userId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
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
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    public async Task<List<CoursePreviewDto>> GetCoursesByStudentId(Guid studentId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.Teacher)
            .Where(c => c.CourseStudents.Any(cs => cs.StudentId == studentId))
            .ToListAsync();

        return _mapper.Map<List<CoursePreviewDto>>(courses);
    }

    // -----------------
    // TEACHER FUNCTIONS
    // -----------------

    public async Task AddStudentToCourse(Guid teacherId, Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.TeacherId != teacherId)
            throw new UnauthorizedAccessException("Not allowed");

        var studentExists = await _dbContext.Users
            .AnyAsync(u => u.Id == studentId && u.Role == UserRole.Student);

        if (!studentExists)
            throw new KeyNotFoundException("Student not found");

        var alreadyEnrolled = await _dbContext.CourseStudents
            .AnyAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

        if (alreadyEnrolled)
            throw new InvalidOperationException("Student already enrolled");

        _dbContext.CourseStudents.Add(new CourseStudent
        {
            CourseId = courseId,
            StudentId = studentId
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveStudentFromCourse(Guid teacherId, Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (course.TeacherId != teacherId)
            throw new UnauthorizedAccessException("Not allowed");

        var relation = await _dbContext.CourseStudents
            .FirstOrDefaultAsync(cs =>
                cs.CourseId == courseId &&
                cs.StudentId == studentId);

        if (relation == null)
            throw new InvalidOperationException("Student not enrolled");

        _dbContext.CourseStudents.Remove(relation);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsStudentEnrolled(Guid courseId, Guid studentId)
    {
        return await _dbContext.CourseStudents
            .AnyAsync(cs =>
                cs.CourseId == courseId &&
                cs.StudentId == studentId);
    }

    public async Task ClearCourseStudents(Guid courseId)
    {
        var relations = _dbContext.CourseStudents
            .Where(cs => cs.CourseId == courseId);

        _dbContext.CourseStudents.RemoveRange(relations);

        await _dbContext.SaveChangesAsync();
    }
}