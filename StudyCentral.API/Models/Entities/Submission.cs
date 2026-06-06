namespace StudyCentral.API.Models.Entities;

public class Submission
{
    public Guid Id { get; set; }
    
    // Submission
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public string? Comment { get; set; }
    public SubmissionStatus Status { get; set; } = SubmissionStatus.NotSubmitted;
    
    // Review
    public string? Feedback { get; set; }
    public GradeLetter? Grade { get; set; }
    public DateTime? GradedAt { get; set; }
    
    // Files
    public ICollection<StudyFile> StudyFiles { get; set; } = new List<StudyFile>();
    
    // Assignment
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    
    // Student
    public Guid StudentId { get; set; }
    public User Student { get; set; } = null!;
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum GradeLetter
{
    A,
    B,
    C,
    D,
    F
}

public enum SubmissionStatus
{
    NotSubmitted,
    Submitted,
    SubmittedLate,
    Passed,
    Failed
}