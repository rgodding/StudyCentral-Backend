namespace StudyCentral.API.Models.Entities;

public class Announcement
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    // Course
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    // Attachments
    public ICollection<StudyFile> Files { get; set; } = new List<StudyFile>();
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}