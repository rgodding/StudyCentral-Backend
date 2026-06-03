using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Submission;

public class CreateSubmissionDto
{
    [MaxLength(2000)]
    public string? Comment { get; set; }

    [Required]
    public Guid AssignmentId { get; set; }
}