using StudyCentral.API.Models.DTOs.User;

namespace StudyCentral.API.Models.DTOs.Course;

public class CoursePreviewDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public UserPreviewDto? Teacher { get; set; }
    public int StudentCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}