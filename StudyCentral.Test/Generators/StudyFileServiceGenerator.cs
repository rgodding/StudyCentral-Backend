using Microsoft.AspNetCore.Http;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;

namespace StudyCentral.Test.Generators;

public class StudyFileServiceGenerator : IStudyFileService
{
    public Task<BlobFileResult> DownloadFile(
        Guid userId,
        Guid fileId)
    {
        return Task.FromResult(new BlobFileResult
        {
            Content = Stream.Null,
            ContentType = "application/pdf",
            FileName = "test.pdf"
        });
    }

    public Task<StudyFile> UploadFile(
        IFormFile file,
        Guid userId,
        string? altText = null)
    {
        return Task.FromResult(new StudyFile
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            BlobName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
            AltText = altText,
            UploadedById = userId
        });
    }

    public Task DeleteFile(Guid fileId)
    {
        return Task.CompletedTask;
    }

    public Task<List<StudyFileDto>> GetFilesByFolderId(Guid folderId)
    {
        return Task.FromResult(new List<StudyFileDto>());
    }

    public Task AttachToFolder(Guid fileId, Guid folderId)
    {
        return Task.CompletedTask;
    }

    public Task RemoveFromFolder(Guid fileId)
    {
        return Task.CompletedTask;
    }

    public Task<List<StudyFileDto>> GetFilesByAnnouncementId(Guid announcementId)
    {
        return Task.FromResult(new List<StudyFileDto>());
    }

    public Task AttachToAnnouncement(Guid fileId, Guid announcementId)
    {
        return Task.CompletedTask;
    }

    public Task RemoveFromAnnouncement(Guid fileId, Guid announcementId)
    {
        return Task.CompletedTask;
    }

    public Task<List<StudyFileDto>> GetFilesByAssignmentId(Guid assignmentId)
    {
        return Task.FromResult(new List<StudyFileDto>());
    }

    public Task AttachToAssignment(Guid fileId, Guid assignmentId)
    {
        return Task.CompletedTask;
    }

    public Task RemoveFromAssignment(Guid fileId, Guid assignmentId)
    {
        return Task.CompletedTask;
    }

    public Task<List<StudyFileDto>> GetFilesBySubmissionId(Guid submissionId)
    {
        return Task.FromResult(new List<StudyFileDto>());
    }

    public Task AttachToSubmission(Guid fileId, Guid submissionId)
    {
        return Task.CompletedTask;
    }

    public Task RemoveFromSubmission(Guid fileId, Guid submissionId)
    {
        return Task.CompletedTask;
    }
}