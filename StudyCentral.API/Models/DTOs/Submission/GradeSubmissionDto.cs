using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.DTOs.Submission;

public class GradeSubmissionDto
{
    public GradeLetter Grade { get; set; }
    public string? Feedback { get; set; }
}