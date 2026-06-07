namespace StudyCentral.API.Models.DTOs.Admin.Course;

public class AdminUpdateCourseDto
{
    public string? Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public Guid? TeacherId { get; set; }
}