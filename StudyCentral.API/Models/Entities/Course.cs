using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.API.Models.Entities;

public class Course
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Teacher
    public Guid? TeacherId { get; set; }
    public User? Teacher { get; set; }
    
    // Students
    public ICollection<CourseStudent> CourseStudents { get; set; }
        = new List<CourseStudent>();

    // Course content
    public ICollection<Assignment> Assignments { get; set; }
        = new List<Assignment>();

    public ICollection<Announcement> Announcements { get; set; }
        = new List<Announcement>();
   
    public ICollection<StudyFolder> StudyFolders { get; set; }
        = new List<StudyFolder>();
    
    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public override string ToString()
    {
        return $"Course {{ " +
               $"Id = {Id}, " +
               $"Name = {Name}, " +
               $"Description = {Description}, " +
               $"TeacherId = {TeacherId}, " +
               $"StudentCount = {CourseStudents.Count}, " +
               $"AssignmentCount = {Assignments.Count}, " +
               $"AnnouncementCount = {Announcements.Count}, " +
               $"FolderCount = {StudyFolders.Count}, " +
               $"CreatedAt = {CreatedAt:o}, " +
               $"UpdatedAt = {UpdatedAt:o} " +
               $"}}";
    }
}