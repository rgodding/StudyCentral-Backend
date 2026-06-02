namespace StudyCentral.API.Models.Dtos.Submissions;

public class UpdateSubmissionDto
{
    public string? Comment { get; set; }
    public string? Feedback { get; set; }
    public int? Grade { get; set; }
}