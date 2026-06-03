using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Submission;

public class GradeSubmissionDto
{
    [MaxLength(2000)]
    public string? Feedback { get; set; }

    public decimal? Grade { get; set; }
}