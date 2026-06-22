namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobStorageItemDto
{
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public double SizeMb { get; set; }
    public DateTime? LastModified { get; set; }
    public string FileExtension { get; set; } = string.Empty;
}