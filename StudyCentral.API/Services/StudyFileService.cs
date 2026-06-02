using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Services;

public interface IStudyFileService
{
    Task<StudyFile> GetFileById(Guid fileId);
    Task<List<StudyFile>> GetFilesByCourseId(Guid courseId);
    Task<List<StudyFile>> GetFilesByAssignmentId(Guid assignmentId);
    Task<List<StudyFile>> GetFilesBySubmissionId(Guid submissionId);
    
    Task<StudyFile> UploadCourseFile(Guid courseId, Guid userId, IFormFile file);
    Task<StudyFile> UploadAssignmentFile(Guid assignmentId, Guid userId, IFormFile file);
    Task<StudyFile> UploadSubmissionFile(Guid submissionId, Guid userId, IFormFile file);
    Task<StudyFile> UploadProfilePicture(Guid userId, IFormFile file, string? altText = "image");
    
    Task<List<StudyFile>> GetFilesByUserId(Guid userId);
    
    Task DeleteFileById(Guid fileId);
}

public class StudyFileService : IStudyFileService
{
    public Task<StudyFile> GetFileById(Guid fileId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StudyFile>> GetFilesByCourseId(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StudyFile>> GetFilesByAssignmentId(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<StudyFile>> GetFilesBySubmissionId(Guid submissionId)
    {
        throw new NotImplementedException();
    }

    public Task<StudyFile> UploadCourseFile(Guid courseId, Guid userId, IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<StudyFile> UploadAssignmentFile(Guid assignmentId, Guid userId, IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<StudyFile> UploadSubmissionFile(Guid submissionId, Guid userId, IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<StudyFile> UploadProfilePicture(Guid userId, IFormFile file, string? altText = "image")
    {
        throw new NotImplementedException();
    }

    public Task<List<StudyFile>> GetFilesByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFileById(Guid fileId)
    {
        throw new NotImplementedException();
    }
}