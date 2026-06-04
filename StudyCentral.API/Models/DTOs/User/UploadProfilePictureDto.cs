namespace StudyCentral.API.Models.DTOs.User;

public class UploadProfilePictureDto
{
    public IFormFile File { get; set; } = null!;
    public string? FileName { get; set; }
    public string? AltText { get; set; }
}