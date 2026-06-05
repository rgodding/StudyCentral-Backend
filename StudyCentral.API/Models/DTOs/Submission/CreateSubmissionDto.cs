using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Submission;

public class CreateSubmissionDto
{
    [Required]
    public Guid AssignmentId { get; set; }
    
    [MaxLength(2000)]
    public string? Comment { get; set; }
    
    [Required]
    public Guid StudentId { get; set; }
}