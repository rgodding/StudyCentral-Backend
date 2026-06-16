using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.Entities;

public class StudyFile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FileName { get; set; } = null!;
    public string BlobName { get; set; } = null!;

    public FileType FileType { get; set; }

    public string ContentType { get; set; } = null!;
    public long Size { get; set; }

    public string? AltText { get; set; }
    
    // Folder
    public Guid? StudyFolderId { get; set; }
    public StudyFolder? StudyFolder { get; set; }

    // Assignment
    public Guid? AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }

    // Announcement
    public Guid? AnnouncementId { get; set; }
    public Announcement? Announcement { get; set; }

    // Submission
    public Guid? SubmissionId { get; set; }
    public Submission? Submission { get; set; }
    
    // Audit
    public Guid UploadedById { get; set; }
    public User UploadedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}