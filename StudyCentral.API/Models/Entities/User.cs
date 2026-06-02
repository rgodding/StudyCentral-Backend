namespace StudyCentral.API.Models.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public UserRole Role { get; set; }
    
    public Guid? ProfilePictureId { get; set; }
    public StudyFile? ProfilePicture { get; set; }
    
    public ICollection<StudyFile> UploadedFiles { get; set; } = new List<StudyFile>();
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Student relationships
    public ICollection<Course> EnrolledCourses { get; set; } = new List<Course>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    
    // Teacher relationships
    public ICollection<Course> TeachingCourses { get; set; } = new List<Course>();
    public ICollection<Assignment> CreatedAssignments { get; set; } = new List<Assignment>();
    public ICollection<Announcement> CreatedAnnouncements { get; set; } = new List<Announcement>();
}

public enum UserRole
{
    User,
    Student,
    Teacher,
    Admin
}