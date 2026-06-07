using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Admin.Course;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Services;

public interface ICourseService
{
    // CRUD
    Task<List<CourseDto>> GetAll();
    Task<CourseDto> GetById(Guid courseId);
    Task<CourseDto> Create(CreateCourseDto dto);
    Task<CourseDto> Update(Guid courseId, UpdateCourseDto dto);
    Task Delete(Guid courseId);

    // Admin Methods
    Task<CourseDto> AdminUpdateCourse(Guid courseId, AdminUpdateCourseDto dto);

    // Teacher Methods
    Task<List<CourseDto>> GetCoursesByTeacherId(Guid teacherId);
    Task<CourseDto> GetCourseByTeacherId(Guid teacherId, Guid courseId);
    Task<CourseDto> UpdateCourseByTeacherId(Guid teacherId, Guid courseId, UpdateCourseDto dto);
    Task<List<UserDto>> GetStudentsByTeacherId(Guid teacherId, Guid courseId);
    Task AddStudentByTeacherId(Guid teacherId, Guid courseId, Guid studentId);
    Task RemoveStudentByTeacherId(Guid teacherId, Guid courseId, Guid studentId);

    // Student Methods
    Task<List<CourseDto>> GetCoursesByStudentId(Guid studentId);
    Task<CourseDto> GetCourseByStudentId(Guid studentId, Guid courseId);
    Task<List<UserDto>> GetStudentsByCourseId(Guid studentId, Guid courseId);
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

    // --------------
    //  CRUD METHODS
    // --------------

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

    // ----------------
    // ADMIN METHODS
    // ----------------
    public async Task<CourseDto> AdminUpdateCourse(Guid courseId, AdminUpdateCourseDto dto)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        if (dto.TeacherId != null)
        {
            var teacherExists = await _dbContext.Users
                .AnyAsync(u => u.Id == dto.TeacherId && u.Role == UserRole.Teacher);

            if (!teacherExists)
                throw new KeyNotFoundException("Teacher not found");

            course.TeacherId = dto.TeacherId.Value;
        }

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        course.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    // ----------------
    // TEACHER METHODS
    // ----------------

    public async Task<List<CourseDto>> GetCoursesByTeacherId(Guid teacherId)
    {
        var courses = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .Where(c => c.TeacherId == teacherId)
            .ToListAsync();
        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<CourseDto> GetCourseByTeacherId(
        Guid teacherId,
        Guid courseId)
    {
        var course = await _dbContext.Courses
            .Include(c => c.CourseStudents)
            .FirstOrDefaultAsync(c =>
                c.Id == courseId &&
                c.TeacherId == teacherId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> UpdateCourseByTeacherId(
        Guid teacherId,
        Guid courseId,
        UpdateCourseDto dto)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c =>
                c.Id == courseId &&
                c.TeacherId == teacherId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        course.Name = dto.Name ?? course.Name;
        course.Description = dto.Description ?? course.Description;
        course.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<List<UserDto>> GetStudentsByTeacherId(Guid teacherId, Guid courseId)
    {
        var students = await _dbContext.CourseStudents
            .Where(cs => cs.CourseId == courseId)
            .Select(cs => cs.Student)
            .ToListAsync();
        return _mapper.Map<List<UserDto>>(students);
    }

    public async Task AddStudentByTeacherId(Guid teacherId, Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c =>
                c.Id == courseId &&
                c.TeacherId == teacherId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        var student = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == UserRole.Student);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var enrollmentExists = await _dbContext.CourseStudents
            .AnyAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

        if (enrollmentExists)
            throw new InvalidOperationException("Student is already enrolled in the course");

        _dbContext.CourseStudents.Add(new CourseStudent
        {
            CourseId = courseId,
            StudentId = studentId
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveStudentByTeacherId(Guid teacherId, Guid courseId, Guid studentId)
    {
        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(c =>
                c.Id == courseId &&
                c.TeacherId == teacherId);

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        var enrollment = await _dbContext.CourseStudents
            .FirstOrDefaultAsync(cs => cs.CourseId == courseId && cs.StudentId == studentId);

        if (enrollment == null)
            throw new KeyNotFoundException("Student not found");

        _dbContext.CourseStudents.Remove(enrollment);
        await _dbContext.SaveChangesAsync();
    }

    // ----------------
    // STUDENT METHODS
    // ----------------
    public async Task<List<CourseDto>> GetCoursesByStudentId(Guid studentId)
    {
        var courses = await _dbContext.CourseStudents
            .Where(cs => cs.StudentId == studentId)
            .Select(cs => cs.Course)
            .ToListAsync();
        return _mapper.Map<List<CourseDto>>(courses);
    }

    public async Task<CourseDto> GetCourseByStudentId(Guid studentId, Guid courseId)
    {
        var course = await _dbContext.CourseStudents
            .Where(cs => cs.StudentId == studentId && cs.CourseId == courseId)
            .Select(cs => cs.Course)
            .FirstOrDefaultAsync();

        if (course == null)
            throw new KeyNotFoundException("Course not found");

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<List<UserDto>> GetStudentsByCourseId(Guid studentId, Guid courseId)
    {
        var students = await _dbContext.CourseStudents
            .Where(cs => cs.CourseId == courseId)
            .Select(cs => cs.Student)
            .ToListAsync();
        return _mapper.Map<List<UserDto>>(students);
    }
}