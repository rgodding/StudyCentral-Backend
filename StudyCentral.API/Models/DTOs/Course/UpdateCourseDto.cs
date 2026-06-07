using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Course;

public class UpdateCourseDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}