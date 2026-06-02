namespace StudyCentral.API.Models.Entities;

public class StudyFile
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string BlobName { get; set; } = null!;

    public FileType FileType { get; set; }

    public string ContentType { get; set; } = null!;
    public long Size { get; set; }

    public string? AltText { get; set; }

    public DateTime UploadedAt { get; set; }
        = DateTime.UtcNow;

    public Guid UploadedById { get; set; }
    public User UploadedBy { get; set; } = null!;
    
    // Optional Owners
    public Guid? CourseId { get; set; }
    public Course? Course { get; set; }
    
    public Guid? AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    
    public Guid? SubmissionId { get; set; }
    public Submission? Submission { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
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