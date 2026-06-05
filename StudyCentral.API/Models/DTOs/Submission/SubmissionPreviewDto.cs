namespace StudyCentral.API.Models.DTOs.Submission;

public class SubmissionPreviewDto
{
    public Guid Id { get; set; }

    public Guid AssignmentId { get; set; }
    public string AssignmentTitle { get; set; } = null!;

    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = null!;

    public decimal? Grade { get; set; }

    public int FileCount { get; set; }

    public DateTime SubmittedAt { get; set; }
}