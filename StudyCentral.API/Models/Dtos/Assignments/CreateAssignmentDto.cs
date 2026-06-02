namespace StudyCentral.API.Models.Dtos.Assignments;

public class CreateAssignmentDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
}