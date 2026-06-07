using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.Submission;

public class GradeSubmissionDto
{
    public GradeLetter Grade { get; set; }
    public string? Feedback { get; set; }
}