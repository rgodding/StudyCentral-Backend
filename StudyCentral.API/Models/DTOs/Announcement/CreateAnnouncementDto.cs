using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Announcement;

public class CreateAnnouncementDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = null!;

    [Required]
    public Guid CourseId { get; set; }
}