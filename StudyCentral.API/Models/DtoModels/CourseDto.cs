namespace StudyCentral.API.Models.DtoModels;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}