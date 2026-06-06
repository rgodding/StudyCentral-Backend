namespace StudyCentral.API.Models.ApiModels.Course;

public class UploadFileToFolderRequest
{
    public IFormFile File { get; set; }
    public string? AltText { get; set; }
}