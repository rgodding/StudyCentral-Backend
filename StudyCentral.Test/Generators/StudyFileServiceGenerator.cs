using Microsoft.AspNetCore.Http;
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
        FileType type,
        string? altText = null)
    {
        return Task.FromResult(new StudyFile
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            BlobName = $"test-blob-{Guid.NewGuid()}",
            ContentType = file.ContentType ?? "image/png",
            UploadedById = userId,
            FileType = type,
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
}