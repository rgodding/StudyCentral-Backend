namespace StudyCentral.API.Models.DTOs.Announcement;

public class AnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int FileCount { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}