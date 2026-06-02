namespace StudyCentral.API.Models.Dtos.Assignments;

public class UpdateAssignmentDto
{
    public string? Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public DateTime? Deadline { get; set; } = null!;
}