namespace StudyCentral.API.Models.Dtos.Submissions;

public class SubmissionDto
{
    public Guid Id { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string? Comment { get; set; }
    public string? Feedback { get; set; }
    public int? Grade { get; set; }
}