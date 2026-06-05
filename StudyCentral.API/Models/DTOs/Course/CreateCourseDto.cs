using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Course;

public class CreateCourseDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public Guid? TeacherId { get; set; }
}