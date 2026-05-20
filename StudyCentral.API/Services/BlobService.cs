using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StudyCentral.API.Middleware;

namespace StudyCentral.API.Services;

public interface IBlobService
{
    // Image functions
    Task<(Stream, string)> GetImage(string url);
    Task<(string, string)> UploadImage(string fileName, IFormFile file);
    Task DeleteImage(string url);
    Task<bool> ImageExists(string filename);
    
    // File functions
    Task<(int, string)> UploadFile(string fileName, IFormFile file);
    Task<(int, string)> DeleteFile(string url);
    Task<bool> FileExists(string filename);
}

public class BlobService : IBlobService
{
    
    private readonly IConfiguration _configuration;

    public BlobService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task<(Stream, string)> GetImage(string url)
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

    public async Task<(string, string)> UploadImage(string fileName, IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        
        var imageExists = await ImageExists(fileName);
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

    public async Task DeleteImage(string url)
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

    public Task<(int, string)> UploadFile(string fileName, IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<(int, string)> DeleteFile(string url)
    {
        throw new NotImplementedException();
    }

    public Task<bool> FileExists(string filename)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> ImageExists(string filename)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(filename);
        return await blobClient.ExistsAsync();
    }

    public int GetBlobCount()
    {
        var containerClient = GetBlobContainerClient();
        var blobs = containerClient.GetBlobs();
        return blobs.Count();
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