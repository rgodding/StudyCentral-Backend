using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StudyCentral.API.Middleware;

namespace StudyCentral.API.Services;

public interface IBlobService
{
    // Image functions
    Task<(Stream, string)> GetFile(string url);
    Task<(string, string)> UploadFile(string fileName, IFormFile file);
    Task DeleteFile(string url);
    Task<bool> FileExists(string filename);
    Task<int> GetBlobCount();
    Task<bool> Wipe();
}

public class BlobService : IBlobService
{
    
    private readonly IConfiguration _configuration;

    public BlobService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<(Stream, string)> GetFile(string url)
    {
        if (url == null)
        {
            throw new Exception("Image URL is null");
        }
        
        var containerClient = GetBlobContainerClient();

        try
        {
            var blobClient = containerClient.GetBlobClient(url);
            var response = await blobClient.DownloadAsync();
            return (response.Value.Content, response.Value.ContentType);
        }
        catch (Exception ex)
        {
            var defaultImage = await File.ReadAllBytesAsync("wwwroot/images/default-image.png");
            return (new MemoryStream(defaultImage), "image/png");
        }
    }

    public async Task<(string, string)> UploadFile(string fileName, IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        
        var imageExists = await FileExists(fileName);
        if (imageExists)
        {
            throw new ExceptionMiddleware.ConflictException($"Image with name {fileName} already exists");
        }
        
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);
        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = file.ContentType,
        };
        await blobClient.UploadAsync(new MemoryStream(fileBytes), blobHttpHeader);
        return (fileName, file.ContentType);
    }

    public async Task DeleteFile(string url)
    {
        try
        {
            var containerClient = GetBlobContainerClient();
            var blobClient = containerClient.GetBlobClient(url);
            var response = await blobClient.DeleteIfExistsAsync();
        } catch (Exception ex)
        {
            throw new Exception("Image not found");
        }
    }
    
    
    public async Task<bool> FileExists(string filename)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(filename);
        return await blobClient.ExistsAsync();
    }

    public async Task<int> GetBlobCount()
    {
        var containerClient = GetBlobContainerClient();
        var blobs =  containerClient.GetBlobs();
        return await Task.FromResult(blobs.Count());
    }

    public Task<bool> Wipe()
    {
        var containerClient = GetBlobContainerClient();
        var blobs = containerClient.GetBlobs();
        foreach (var blob in blobs)
        {
            var blobClient = containerClient.GetBlobClient(blob.Name);
            blobClient.DeleteIfExists();
        }

        return Task.FromResult(true);
    }
    
    private BlobContainerClient GetBlobContainerClient()
    {
        var connectionString = _configuration["Azurite:ConnectionString"];
        var containerName = _configuration["Azurite:Container"];
        Console.WriteLine("Connecting to Azure Blob Storage");
        Console.WriteLine($"Container: {containerName}");
        Console.WriteLine($"Connection String: {connectionString}");
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
        return containerClient;
    }
}