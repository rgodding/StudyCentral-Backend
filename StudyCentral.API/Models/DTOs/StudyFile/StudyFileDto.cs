using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.StudyFile;

public class StudyFileDto
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;
    public string BlobName { get; set; } = null!;
    
    public FileType FileType { get; set; }
    
    public string ContentType { get; set; } = null!;
    public long FileSize { get; set; }
    
    public string? AltText { get; set; }
    
    public Guid UploadedById { get; set; }
    public string UploadedByName { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}