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
    public Task GetAssignmentSubmissions(Guid assignmentId);
    public Task GetAssignmentSubmissionById(Guid id);
    public Task GradeAssignmentSubmission(Guid assignmentId, Guid submissionId, GradeSubmissionRequestModel request);
}

public class TeacherService : ITeacherService
{
    private readonly StudyDbContext _dbContext;
    
    public Task<List<Course>> GetCoursesByTeacherId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Course> GetCourseById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetCourseStudentsById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCourse(Guid id, UpdateCourseRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task AddStudentToCourse(Guid courseId, Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Assignment>> GetCourseAssignments(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Assignment>> GetAssignmentsByTeacherId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Assignment> GetAssignmentById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAssignment(Guid courseId, CreateAssignmentRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAssignment(Guid id, UpdateAssignmentRequestModel request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAssignment(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task GetAssignmentSubmissions(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Task GetAssignmentSubmissionById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task GradeAssignmentSubmission(Guid assignmentId, Guid submissionId, GradeSubmissionRequestModel request)
    {
        throw new NotImplementedException();
    }
}