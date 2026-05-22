using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.ApiModels.CourseModels;
using StudyCentral.API.Models.ApiModels.TeacherModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ITeacherService
{
    // Course functions
    public Task<List<Course>> GetCoursesByTeacherId(Guid userId);
    public Task<Course> GetCourseById(Guid id);
    public Task<List<User>> GetCourseStudentsById(Guid id);
    public Task UpdateCourse(Guid id, UpdateCourseRequestModel request);
    public Task AddStudentToCourse(Guid courseId, Guid studentId);
    public Task<List<Assignment>> GetCourseAssignments(Guid courseId);
    
    // Assignment functions
    public Task<List<Assignment>> GetAssignmentsByTeacherId(Guid userId);
    public Task<Assignment> GetAssignmentById(Guid id);
    public Task CreateAssignment(Guid courseId, CreateAssignmentRequestModel request);
    public Task UpdateAssignment(Guid id, UpdateAssignmentRequestModel request);
    public Task DeleteAssignment(Guid id);
    public Task<List<Submission>> GetAssignmentSubmissions(Guid assignmentId);
    public Task<Submission> GetAssignmentSubmissionById(Guid id);
    public Task GradeAssignmentSubmission(Guid assignmentId, Guid submissionId, GradeSubmissionRequestModel request);
}

public class TeacherService : ITeacherService
{
    private readonly StudyDbContext _dbContext;

    public TeacherService(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Course>> GetCoursesByTeacherId(Guid userId)
    {
        var teacher = await _dbContext.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        
        return teacher.Courses.ToList();
    }

    public async Task<Course> GetCourseById(Guid id)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id)
                     ?? throw new KeyNotFoundException($"Course with ID {id} not found.");
        return course;
    }

    public async Task<List<User>> GetCourseStudentsById(Guid id)
    {
        var students = await _dbContext.Users
            .Where(u => u.Courses.Any(c => c.Id == id))
            .ToListAsync()
            ?? throw new KeyNotFoundException($"Course with ID {id} not found.");
        
        return students;
    }

    public async Task UpdateCourse(Guid id, UpdateCourseRequestModel request)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException($"Course with ID {id} not found.");
        
        course.Title = request.Title ?? course.Title;
        course.Description = request.Description ?? course.Description;
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddStudentToCourse(Guid courseId, Guid studentId)
    {
        var student = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == studentId)
            ?? throw new KeyNotFoundException($"Student with ID {studentId} not found.");
        
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new KeyNotFoundException($"Course with ID {courseId} not found.");
        
        student.Courses.Add(course);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Assignment>> GetCourseAssignments(Guid courseId)
    {
        var course = await _dbContext.Courses
                         .Include(c => c.Assignments)
                         .FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new KeyNotFoundException($"Course with ID {courseId} not found.");
        
        return course.Assignments.ToList();
        
    }

    public async Task<List<Assignment>> GetAssignmentsByTeacherId(Guid userId)
    {
        var teacher = await _dbContext.Users
            .Include(u => u.Assignments)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        
        return teacher.Assignments.ToList();
    }

    public async Task<Assignment> GetAssignmentById(Guid id)
    {
        var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new KeyNotFoundException($"Assignment with ID {id} not found.");
        
        return assignment;
    }

    public async Task CreateAssignment(Guid courseId, CreateAssignmentRequestModel request)
    {
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId)
            ?? throw new KeyNotFoundException($"Course with ID {courseId} not found.");
        
        var assignment = new Assignment
        {
            Title = request.Title,
            Description = request.Description,
            Deadline = request.Deadline,
            Course = course
        };
        
        _dbContext.Assignments.Add(assignment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAssignment(Guid id, UpdateAssignmentRequestModel request)
    {
        var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new KeyNotFoundException($"Assignment with ID {id} not found.");
        
        assignment.Title = request.Title ?? assignment.Title;
        assignment.Description = request.Description ?? assignment.Description;
        assignment.Deadline = request.Deadline ?? assignment.Deadline;
        
        await _dbContext.SaveChangesAsync();   
    }

    public async Task DeleteAssignment(Guid id)
    {
        var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new KeyNotFoundException($"Assignment with ID {id} not found.");
        
        _dbContext.Assignments.Remove(assignment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Submission>> GetAssignmentSubmissions(Guid assignmentId)
    {
        var submissions = await _dbContext.Submissions
            .Where(s => s.AssignmentId == assignmentId)
            .ToListAsync();
        
        return submissions;
    }

    public async Task<Submission> GetAssignmentSubmissionById(Guid id)
    {
        var submission = await _dbContext.Submissions.FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new KeyNotFoundException($"Submission with ID {id} not found.");
        
        return submission;
    }

    public async Task GradeAssignmentSubmission(Guid assignmentId, Guid submissionId, GradeSubmissionRequestModel request)
    {
        var submission = await _dbContext.Submissions.FirstOrDefaultAsync(s => s.Id == submissionId)
            ?? throw new KeyNotFoundException($"Submission with ID {submissionId} not found.");
        
        submission.Grade = request.Grade;
        submission.Feedback = request.Feedback;
        
        await _dbContext.SaveChangesAsync();
    }
}