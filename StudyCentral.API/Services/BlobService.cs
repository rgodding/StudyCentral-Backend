namespace StudyCentral.API.Services;

public interface IBlobService
{
    // Image functions
    Task<Stream> GetImage(string url);
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
    
    private readonly IConfiguration Configuration;
    
    public BlobService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public Task<Stream> GetImage(string url)
    {
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
}