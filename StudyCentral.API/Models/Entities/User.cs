namespace StudyCentral.API.Models.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public UserRole Role { get; set; }
    
    public ImageFile? ProfilePicture { get; set; }
    
    // Course related relationships
    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    
    
    // Chat related relationships
    public ICollection<Chat> Chats { get; set; } = new List<Chat>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    
}

public enum UserRole
{
    Student,
    Teacher,
    Admin
}