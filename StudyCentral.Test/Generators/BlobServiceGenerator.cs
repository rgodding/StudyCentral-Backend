using Microsoft.AspNetCore.Http;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.Test.Generators;

public class FakeBlobService : IBlobService
{
    public Task<BlobUploadResult> UploadFile(string fileName, IFormFile file)
        => Task.FromResult(new BlobUploadResult
        {
            FileName = fileName,
            BlobName = "test-blob",
            ContentType = "image/png"
        });

    public Task<BlobFileResult> GetFile(string blobName)
        => throw new NotImplementedException();

    public Task DeleteFile(string blobName)
        => Task.CompletedTask;

    public Task<bool> FileExists(string blobName)
        => Task.FromResult(true);

    public Task<int> GetBlobCount()
        => Task.FromResult(0);

    public Task<bool> Wipe()
        => Task.FromResult(true);
}