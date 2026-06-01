using StudyCentral.API.Models.Dtos.Users;

namespace StudyCentral.API.Models.Dtos.Courses;

public class UpdateCourseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? TeacherId { get; set; }
}