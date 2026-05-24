using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.Dtos.Courses;

public class CreateCourseDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }
}