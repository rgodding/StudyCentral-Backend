using StudyCentral.API.Models.Dtos.Submissions;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface ISubmissionService
{
    Task<Submission> GetSubmissionById(Guid submissionId);
    Task<List<Submission>> GetSubmissionsByCourseId(Guid courseId);
    Task<List<Submission>> GetSubmissionsByAssessmentId(Guid assessmentId);
    Task<List<Submission>> GetSubmissionsByStudentId(Guid studentId);
    
    Task<Submission> CreateSubmission(Guid studentId, Guid submissionId, CreateSubmissionDto request);
    Task<Submission> UpdateSubmission(Guid studentId, Guid submissionId, UpdateSubmissionDto request);
    Task DeleteSubmission(Guid studentId, Guid submissionId);
}

public class SubmissionService : ISubmissionService
{
    public Task<Submission> GetSubmissionById(Guid submissionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Submission>> GetSubmissionsByCourseId(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Submission>> GetSubmissionsByAssessmentId(Guid assessmentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Submission>> GetSubmissionsByStudentId(Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task<Submission> CreateSubmission(Guid studentId, Guid submissionId, CreateSubmissionDto request)
    {
        throw new NotImplementedException();
    }

    public Task<Submission> UpdateSubmission(Guid studentId, Guid submissionId, UpdateSubmissionDto request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSubmission(Guid studentId, Guid submissionId)
    {
        throw new NotImplementedException();
    }
}