using System.ComponentModel.DataAnnotations;
using StudyCentral.API.Models.Dtos.Users;

namespace StudyCentral.API.Models.Dtos.Courses;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public UserPreviewDto? Teacher { get; set; }
}