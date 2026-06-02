namespace StudyCentral.API.Models.Entities;

public class Announcement
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; }
    

    // Course
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // Creator
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;
}