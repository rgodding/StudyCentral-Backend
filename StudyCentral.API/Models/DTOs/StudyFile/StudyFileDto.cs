using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

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

    // Ownership
    public FileOwnerType? OwnerType { get; set; }
    public Guid? OwnerId { get; set; }

    // Uploader
    public Guid UploadedById { get; set; }
    public string UploadedByName { get; set; } = null!;

    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum FileOwnerType
{
    Folder,
    Assignment,
    Announcement,
    Submission
}