using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.ApiModels.Submission;

public class GradeSubmissionRequest
{
    public GradeLetter Grade { get; set; }
    public string? Feedback { get; set; }
}