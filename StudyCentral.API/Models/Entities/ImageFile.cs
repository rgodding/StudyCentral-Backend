namespace StudyCentral.API.Models.Entities;

public class ImageFile
{
    public Guid Id { get; set; }
    
    public string FileName { get; set; } = null!;
    public string AltText { get; set; } = "image";

    public DateTime UploadedAt { get; set; }
}