namespace StudyCentral.API.Models.Dtos.Announcements;

public class CreateAnnouncementDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}