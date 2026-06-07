namespace StudyCentral.API.Models.DTOs.Admin.Announcement;

public class AdminUpdateAnnouncementDto
{
    public string? Name { get; set; } = null!;

    public string? Content { get; set; } = null!;

    public Guid? CourseId { get; set; }
}