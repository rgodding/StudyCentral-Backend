namespace StudyCentral.API.Models.DTOs.Submission;

public class SubmissionDto
{
    public Guid Id { get; set; }
    public string? Comment { get; set; }
    public string? Feedback { get; set; }
    public decimal? Grade { get; set; }
    
    public Guid AssignmentId { get; set; }
    public string AssignmentTitle { get; set; } = null!;
    
    public Guid StudentId { get; set; }
    public string StudentFirstName { get; set; } = null!;
    public string? StudentLastName { get; set; } = null!;
    public string? StudentProfilePictureUrl { get; set; }
    
    public int FileCount { get; set; }
    
    public DateTime SubmittedAt { get; set; }
    public DateTime? GradedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}