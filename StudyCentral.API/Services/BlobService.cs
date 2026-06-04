using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StudyCentral.API.Models.DTOs.StudyFile;

namespace StudyCentral.API.Services;

public interface IBlobService
{
    // Core operations
    Task<BlobFileResult> GetFile(string blobName);
    Task<BlobUploadResult> UploadFile(string fileName, IFormFile file);
    Task DeleteFile(string blobName);

    // Utilities
    Task<bool> FileExists(string blobName);

    Task<int> GetBlobCount();

    Task<bool> Wipe();
}
public class BlobService : IBlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IConfiguration configuration)
    {
        var connectionString = configuration["Azurite:ConnectionString"];
        var containerName = configuration["Azurite:Container"];
        
        var client = new BlobServiceClient(connectionString);
        _containerClient = client.GetBlobContainerClient(containerName);
        
        _containerClient.CreateIfNotExists();
    }

    public async Task<BlobFileResult> GetFile(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        
        var exists = await blobClient.ExistsAsync();
        if (!exists)
        {
            throw new FileNotFoundException($"Blob with name '{blobName}' not found");
        }
        
        var download = await blobClient.DownloadAsync();

        return new BlobFileResult
        {
            Content = download.Value.Content,
            ContentType = download.Value.ContentType,
            FileName = blobClient.Name
        };
    }

    public async Task<BlobUploadResult> UploadFile(string fileName, IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is null or empty");
        
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is null or empty");

        // Prevents path injection
        var safeFileName = Path.GetFileName(fileName);
        
        // Unique blob name
        var blobName = $"{Guid.NewGuid()}_{safeFileName.Replace(" ", "_")}";
        
        var blobClient = _containerClient.GetBlobClient(blobName);
        
        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = file.ContentType
        };

        using var stream = file.OpenReadStream();
        
        await blobClient.UploadAsync(stream, httpHeaders);

        return new BlobUploadResult
        {
            FileName = safeFileName,
            BlobName = blobName,
            ContentType = file.ContentType
        };
    }

    public async Task DeleteFile(string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new ArgumentException("Blob name is null or empty");

        var blobClient = _containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<bool> FileExists(string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new ArgumentException("Blob name is null or empty");

        var blobClient = _containerClient.GetBlobClient(blobName);

        var response = await blobClient.ExistsAsync();

        return response.Value;
    }

    public async Task<int> GetBlobCount()
    {
        int count = 0;

        await foreach (var blob in _containerClient.GetBlobsAsync())
        {
            count++;
        }

        return count;
    }

    public async Task<bool> Wipe()
    {
        try
        {
            await foreach (var blob in _containerClient.GetBlobsAsync())
            {
                await _containerClient.DeleteBlobIfExistsAsync(blob.Name);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}