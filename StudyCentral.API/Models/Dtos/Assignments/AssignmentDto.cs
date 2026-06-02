namespace StudyCentral.API.Models.Dtos.Assignments;

public class AssignmentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
}