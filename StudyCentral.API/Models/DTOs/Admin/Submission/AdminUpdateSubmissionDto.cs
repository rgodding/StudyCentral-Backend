using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.DTOs.Admin.Submission;

public class AdminUpdateSubmissionDto
{
    public string? Comment { get; set; }
    public SubmissionStatus? Status { get; set; }
    public string? Feedback { get; set; }
    public GradeLetter? Grade { get; set; }
}