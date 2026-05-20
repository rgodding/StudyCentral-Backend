using Azure.Storage.Blobs;

namespace StudyCentral.API.Services;

public interface IBlobService
{
    // Image functions
    Task<(Stream, string)> GetImage(string url);
    Task<(int, string)> UploadImage(string fileName, IFormFile file);
    Task<(int, string)> DeleteImage(string url);
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
        throw new NotImplementedException();
    }

    public Task<(int, string)> UploadImage(string fileName, IFormFile file)
    {
        throw new NotImplementedException();
    }

    public Task<(int, string)> DeleteImage(string url)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ImageExists(string filename)
    {
        throw new NotImplementedException();
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