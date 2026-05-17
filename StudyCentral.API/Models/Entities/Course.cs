namespace StudyCentral.Data.Entities;

public class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    
    public Guid TeacherId { get; set; }
    public User? Teacher { get; set; }
    
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}