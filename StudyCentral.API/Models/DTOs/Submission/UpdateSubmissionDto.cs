using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Submission;

public class UpdateSubmissionDto
{
    [MaxLength(2000)]
    public string? Comment { get; set; }
}