namespace StudyCentral.API.Models.DtoModels;

public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    
    public UserDto? Teacher { get; set; }
    
    public ICollection<UserDto> Students { get; set; } = new List<UserDto>();
    public ICollection<AssignmentDto> Assignments { get; set; } = new List<AssignmentDto>();
}