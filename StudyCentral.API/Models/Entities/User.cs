using StudyCentral.API.Models.Entities.Relationship;

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
    
    // Student relationships
    public ICollection<CourseStudent> EnrolledCourses { get; set; } = new List<CourseStudent>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    
    // Teacher relationships
    public ICollection<Course> TeachingCourses { get; set; } = new List<Course>();
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum UserRole
{
    Student,
    Teacher,
    Admin
}