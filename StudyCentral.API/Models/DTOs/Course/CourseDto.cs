namespace StudyCentral.API.Models.DTOs.Course;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int StudentCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}