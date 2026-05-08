namespace StudyCentral.Data.Entities;

public class Material
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public string FileUrl { get; set; } = null;
    
    public DateTime UploadedAt { get; set; }
    
    public Guid CourseId { get; set; }
    public Course? Course { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}