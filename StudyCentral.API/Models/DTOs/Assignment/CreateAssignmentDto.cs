using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Assignment;

public class CreateAssignmentDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateTime? Deadline { get; set; }

    [Required]
    public Guid CourseId { get; set; }
}