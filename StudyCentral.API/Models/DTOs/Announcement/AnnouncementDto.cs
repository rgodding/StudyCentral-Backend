namespace StudyCentral.API.Models.DTOs.Announcement;

public class AnnouncementDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int FileCount { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public Guid? TeacherId { get; set; }
    public string? TeacherName { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}