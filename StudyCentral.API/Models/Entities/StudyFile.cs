namespace StudyCentral.API.Models.Entities;

public class StudyFile
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;
    public string BlobName { get; set; } = null!;

    public FileType FileType { get; set; }

    public string ContentType { get; set; } = null!;
    public long Size { get; set; }

    public string? AltText { get; set; }

    public Guid UploadedById { get; set; }
    public User UploadedBy { get; set; } = null!;
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum FileType
{
    Image,
    Video,
    Audio,
    Pdf,
    Document,
    Other
}