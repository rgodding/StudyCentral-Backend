using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Services;


public interface ICourseStudentService
{
    Task EnrollStudent(Guid studentId, Guid courseId);
    Task RemoveStudent(Guid studentId, Guid courseId);
}

public class CourseStudentService : ICourseStudentService
{
    private readonly StudyDbContext _dbContext;
    private readonly IMapper _mapper;

    public CourseStudentService(StudyDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task EnrollStudent(Guid studentId, Guid courseId)
    {
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == studentId);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (student.Role != UserRole.Student)
            throw new InvalidOperationException("User is not a student");

        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
            throw new KeyNotFoundException("Course not found");

        var alreadyEnrolled = await _dbContext.CourseStudents
            .AnyAsync(cs => cs.StudentId == studentId && cs.CourseId == courseId);

        if (alreadyEnrolled)
            throw new InvalidOperationException("Student already enrolled");

        var enrollment = new CourseStudent
        {
            StudentId = studentId,
            CourseId = courseId
        };

        _dbContext.CourseStudents.Add(enrollment);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task RemoveStudent(Guid studentId, Guid courseId)
    {
        var enrollment = await _dbContext.CourseStudents
            .FirstOrDefaultAsync(cs =>
                cs.StudentId == studentId &&
                cs.CourseId == courseId);

        if (enrollment == null)
            throw new KeyNotFoundException("Enrollment not found");

        _dbContext.CourseStudents.Remove(enrollment);
        await _dbContext.SaveChangesAsync();
    }
}