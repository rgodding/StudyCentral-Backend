namespace StudyCentral.API.Models.Entities;

public class Submission
{
    public Guid Id { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    // Student comment
    public string? Comment { get; set; }

    // Teacher feedback
    public string? Feedback { get; set; }

    public decimal? Grade { get; set; }

    // Assignment
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    // Student
    public Guid StudentId { get; set; }
    public User Student { get; set; } = null!;

    // Attached files
    public ICollection<StudyFile> Files { get; set; }
        = new List<StudyFile>();
}