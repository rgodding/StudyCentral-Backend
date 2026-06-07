using Microsoft.AspNetCore.Http;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.Test.Generators;

public class BlobServiceGenerator : IBlobService
{
    public Task<BlobUploadResult> UploadFile(string fileName, IFormFile file)
        => Task.FromResult(new BlobUploadResult
        {
            FileName = fileName,
            BlobName = "test-blob-" + Guid.NewGuid(),
            ContentType = file.ContentType ?? "image/png"
        });

    public Task<BlobFileResult> GetFile(string blobName)
        => Task.FromResult(new BlobFileResult
        {
            FileName = "test-file",
            ContentType = "image/png",
            Content = new MemoryStream()
        });

    public Task DeleteFile(string blobName)
        => Task.CompletedTask;

    public Task<string> GetBlobUrl(string blobName)
        => Task.FromResult($"https://fake-storage/{blobName}");

    public Task<bool> FileExists(string blobName)
        => Task.FromResult(true);

    public Task<int> GetBlobCount()
        => Task.FromResult(1);

    public Task<bool> Wipe()
        => Task.FromResult(true);

    public Task<BlobUploadResult> UploadFileTest(string fileName, IFormFile file, string? blobName = null)
        => Task.FromResult(new BlobUploadResult
        {
            FileName = fileName,
            BlobName = blobName ?? "test-blob-" + Guid.NewGuid(),
            ContentType = file.ContentType ?? "image/png"
        });
}