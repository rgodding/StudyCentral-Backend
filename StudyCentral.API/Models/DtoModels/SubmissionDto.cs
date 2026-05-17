namespace StudyCentral.API.Models.DtoModels;

public class SubmissionDto
{
    public Guid Id { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string? Feedback { get; set; }
    public int Grade { get; set; }
}