namespace StudyCentral.API.Models.DTOs.StudyFile;

public class UploadFileDto
{
    public IFormFile File { get; set; } = null!;
    public string? AltText { get; set; }
}