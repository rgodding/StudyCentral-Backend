namespace StudyCentral.API.Models.Entities;

public class Assignment
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }

    // Course
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // Files
    public ICollection<StudyFile> Files { get; set; }
        = new List<StudyFile>();

    // Student submissions
    public ICollection<Submission> Submissions { get; set; }
        = new List<Submission>();
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}