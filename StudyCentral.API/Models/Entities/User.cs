namespace StudyCentral.API.Models.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public UserRole Role { get; set; }
    
    public StudyFile? Avatar { get; set; }
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

public enum UserRole
{
    Student,
    Teacher,
    Admin
}