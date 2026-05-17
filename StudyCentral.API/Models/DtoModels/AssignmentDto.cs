namespace StudyCentral.API.Models.DtoModels;

public class AssignmentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
}