namespace StudyCentral.API.Models.Entities;

public class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    
    public Guid? TeacherId { get; set; }
    public User? Teacher { get; set; }
    
    public ICollection<User> Students { get; set; } = new List<User>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}