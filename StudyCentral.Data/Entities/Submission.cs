namespace StudyCentral.Data.Entities;

public class Submission
{
    public Guid Id { get; set; }
    
    public string FileUrl { get; set; }
    
    public DateTime SubmittedAt { get; set; }
    
    public string? Feedback { get; set; }
    public int Grade { get; set; }
    
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
}