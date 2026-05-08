namespace StudyCentral.Data.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public UserRole Role { get; set; }
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}