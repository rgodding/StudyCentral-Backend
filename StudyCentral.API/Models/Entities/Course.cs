namespace StudyCentral.API.Models.Entities;

public class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    // Teacher
    public Guid? TeacherId { get; set; }
    public User? Teacher { get; set; }
    
    // Students
    public ICollection<User> Students { get; set; }
        = new List<User>();

    // Course content
    public ICollection<Assignment> Assignments { get; set; }
        = new List<Assignment>();

    public ICollection<Announcement> Announcements { get; set; }
        = new List<Announcement>();
   
    public ICollection<StudyFolder> Folders { get; set; }
        = new List<StudyFolder>();
}