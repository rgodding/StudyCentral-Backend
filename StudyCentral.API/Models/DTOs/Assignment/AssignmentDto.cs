namespace StudyCentral.API.Models.DTOs.Assignment;

public class AssignmentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    
    public int FileCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}