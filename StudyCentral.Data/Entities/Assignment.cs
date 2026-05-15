namespace StudyCentral.Data.Entities;

public class Assignment
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    
    public DateTime Deadline { get; set; }
    
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    
    public ICollection<File> Files { get; set; } = new List<File>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}