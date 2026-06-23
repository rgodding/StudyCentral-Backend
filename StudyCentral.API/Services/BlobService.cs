using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.StudyFile;

namespace StudyCentral.API.Services;

public interface IBlobService
{
    // METHODS
    Task<BlobFileResult> GetFile(string blobName);
    Task<BlobUploadResult> UploadFile(string fileName, IFormFile file);
    Task DeleteFile(string blobName);

    // UTILITIES
    Task<string> GetBlobUrl(string blobName);
    Task<bool> FileExists(string blobName);

    // TEST / ADMINISTRATOR METHODS
    Task<BlobStorageDataDto> GetBlobStorageDataAsync();
    Task<List<BlobStorageItemDto>> GetBlobStorageListAsync();
    Task<BlobStorageHealthDto> GetBlobStorageHealthAsync();
    Task<BlobStorageItemDto> CreateTestBlobAsync();
    Task<int> WipeBlobStorageAsync();
    Task<BlobUploadResult> UploadFileTest(string fileName, IFormFile file, string? blobName = null);
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

    public BlobService(BlobContainerClient containerClient)
    {
        _containerClient = containerClient;
    }

    public async Task<BlobFileResult> GetFile(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);

        var exists = await blobClient.ExistsAsync();
        if (!exists)
            throw new KeyNotFoundException($"Blob with name '{blobName}' not found");

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

        var safeFileName = Path.GetFileName(fileName);
        var blobName = $"{Guid.NewGuid()}_{safeFileName.Replace(" ", "_")}";

        var blobClient = _containerClient.GetBlobClient(blobName);

        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = file.ContentType
        };

        using var stream = file.OpenReadStream();

        await blobClient.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = httpHeaders
        });

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

    public async Task<string> GetBlobUrl(string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new ArgumentException("Blob name is null or empty");

        var blobClient = _containerClient.GetBlobClient(blobName);

        var exists = await blobClient.ExistsAsync();

        if (!exists)
            throw new FileNotFoundException($"Blob '{blobName}' not found");

        return blobClient.Uri.ToString();
    }

    public async Task<bool> FileExists(string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
            throw new ArgumentException("Blob name is null or empty");

        var blobClient = _containerClient.GetBlobClient(blobName);

        var response = await blobClient.ExistsAsync();

        return response.Value;
    }

    public async Task<BlobStorageDataDto> GetBlobStorageDataAsync()
    {
        var blobs = new List<BlobStorageItemDto>();

        await foreach (var blob in _containerClient.GetBlobsAsync())
        {
            blobs.Add(MapBlobItem(blob));
        }

        var totalBytes = blobs.Sum(blob => blob.Size);
        var largestBlob = blobs.OrderByDescending(blob => blob.Size).FirstOrDefault();
        var newestBlob = blobs.OrderByDescending(blob => blob.LastModified).FirstOrDefault();
        var oldestBlob = blobs.OrderBy(blob => blob.LastModified).FirstOrDefault();

        return new BlobStorageDataDto
        {
            ContainerName = _containerClient.Name,
            BlobCount = blobs.Count,
            TotalBytes = totalBytes,
            TotalMb = Math.Round(totalBytes / 1024d / 1024d, 2),
            LargestBlob = largestBlob,
            NewestBlob = newestBlob,
            OldestBlob = oldestBlob
        };
    }

    public async Task<List<BlobStorageItemDto>> GetBlobStorageListAsync()
    {
        var blobs = new List<BlobStorageItemDto>();

        await foreach (var blob in _containerClient.GetBlobsAsync())
        {
            blobs.Add(MapBlobItem(blob));
        }

        return blobs
            .OrderByDescending(blob => blob.LastModified)
            .ToList();
    }

    public async Task<BlobStorageHealthDto> GetBlobStorageHealthAsync()
    {
        var testBlobName = $"health-check-{Guid.NewGuid()}.txt";

        try
        {
            var containerExists = await _containerClient.ExistsAsync();

            var blobClient = _containerClient.GetBlobClient(testBlobName);

            await using var stream = new MemoryStream("health-check"u8.ToArray());

            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = "text/plain"
            });

            var canCreateBlob = await blobClient.ExistsAsync();

            await blobClient.DeleteIfExistsAsync();

            var stillExists = await blobClient.ExistsAsync();

            return new BlobStorageHealthDto
            {
                CanConnect = true,
                ContainerExists = containerExists.Value,
                CanCreateBlob = canCreateBlob.Value,
                CanDeleteBlob = !stillExists.Value,
                Error = null
            };
        }
        catch (Exception ex)
        {
            return new BlobStorageHealthDto
            {
                CanConnect = false,
                ContainerExists = false,
                CanCreateBlob = false,
                CanDeleteBlob = false,
                Error = ex.Message
            };
        }
    }

    public async Task<BlobStorageItemDto> CreateTestBlobAsync()
    {
        var blobName = $"test-blob-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid()}.txt";
        var blobClient = _containerClient.GetBlobClient(blobName);

        var content = $"StudyCentral test blob created at {DateTime.UtcNow:O}";
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        await blobClient.UploadAsync(stream, new BlobHttpHeaders
        {
            ContentType = "text/plain"
        });

        var properties = await blobClient.GetPropertiesAsync();

        return new BlobStorageItemDto
        {
            Name = blobName,
            ContentType = properties.Value.ContentType ?? "text/plain",
            Size = properties.Value.ContentLength,
            SizeMb = Math.Round(properties.Value.ContentLength / 1024d / 1024d, 2),
            LastModified = properties.Value.LastModified.UtcDateTime,
            FileExtension = Path.GetExtension(blobName)
        };
    }

    public async Task<int> WipeBlobStorageAsync()
    {
        var deletedCount = 0;

        await foreach (var blob in _containerClient.GetBlobsAsync())
        {
            var deleted = await _containerClient.DeleteBlobIfExistsAsync(blob.Name);

            if (deleted.Value)
                deletedCount++;
        }

        return deletedCount;
    }

    public async Task<int> GetBlobCount()
    {
        var count = 0;

        await foreach (var _ in _containerClient.GetBlobsAsync())
        {
            count++;
        }

        return count;
    }

    public async Task<bool> Wipe()
    {
        try
        {
            await WipeBlobStorageAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<BlobUploadResult> UploadFileTest(string fileName, IFormFile file, string? blobName = null)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is null or empty");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is null or empty");

        var safeFileName = Path.GetFileName(fileName);

        blobName ??= safeFileName.Replace(" ", "_");

        var blobClient = _containerClient.GetBlobClient(blobName);

        var httpHeaders = new BlobHttpHeaders
        {
            ContentType = file.ContentType
        };

        using var stream = file.OpenReadStream();

        await blobClient.UploadAsync(stream, new BlobUploadOptions
        {
            HttpHeaders = httpHeaders
        });

        return new BlobUploadResult
        {
            FileName = safeFileName,
            BlobName = blobName,
            ContentType = file.ContentType
        };
    }

    private static BlobStorageItemDto MapBlobItem(BlobItem blob)
    {
        var size = blob.Properties.ContentLength ?? 0;

        return new BlobStorageItemDto
        {
            Name = blob.Name,
            ContentType = blob.Properties.ContentType ?? "unknown",
            Size = size,
            SizeMb = Math.Round(size / 1024d / 1024d, 2),
            LastModified = blob.Properties.LastModified?.UtcDateTime,
            FileExtension = Path.GetExtension(blob.Name)
        };
    }
}
