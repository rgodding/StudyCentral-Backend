namespace StudyCentral.API.Models.DTOs.Admin.Assignment;

public class AdminUpdateAssignmentDto
{
    public string? Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public DateTime? Deadline { get; set; }

    public Guid? CourseId { get; set; }
}