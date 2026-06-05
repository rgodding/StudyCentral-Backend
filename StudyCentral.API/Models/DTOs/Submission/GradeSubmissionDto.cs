using System.ComponentModel.DataAnnotations;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.Submission;

public class GradeSubmissionDto
{
    [MaxLength(2000)]
    public string? Feedback { get; set; }

    public GradeLetter? Grade { get; set; }
}