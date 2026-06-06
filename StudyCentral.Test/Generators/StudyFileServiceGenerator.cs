using Microsoft.AspNetCore.Http;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Services;

namespace StudyCentral.Test.Generators;

public class StudyFileServiceGenerator : IStudyFileService
{
    public Task<StudyFile> UploadFile(IFormFile file)
    {
        return Task.FromResult(new StudyFile
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            BlobName = $"test-blob-{Guid.NewGuid()}",
            ContentType = file.ContentType ?? "image/png"
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
            BlobName = $"test-blob-{Guid.NewGuid()}",
            ContentType = file.ContentType ?? "image/png",
            UploadedById = userId,
            FileType = FileType.Image,
            AltText = altText
        });
    }

    public Task DeleteFile(Guid fileId)
    {
        return Task.CompletedTask;
    }

    public Task<string> GetFileUrl(Guid fileId)
    {
        return Task.FromResult(
            $"https://fake-storage/files/{fileId}");
    }

    public async Task<List<StudyFileDto>> GetFilesByFolderId(Guid folderId)
    {
        throw new NotImplementedException();
    }

    public async Task AttachToFolder(Guid fileId, Guid folderId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFromFolder(Guid fileId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StudyFileDto>> GetFilesByAnnouncementId(Guid announcementId)
    {
        throw new NotImplementedException();
    }

    public async Task AttachToAnnouncement(Guid fileId, Guid announcementId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFromAnnouncement(Guid fileId, Guid announcementId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StudyFileDto>> GetFilesByAssignmentId(Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task AttachToAssignment(Guid fileId, Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFromAssignment(Guid fileId, Guid assignmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<StudyFileDto>> GetFilesBySubmissionId(Guid submissionId)
    {
        throw new NotImplementedException();
    }

    public async Task AttachToSubmission(Guid fileId, Guid submissionId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFromSubmission(Guid fileId, Guid submissionId)
    {
        throw new NotImplementedException();
    }
}