namespace StudyCentral.API.Models.Entities.Relationship;

public class CourseStudent
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public Guid StudentId { get; set; }
    public User Student { get; set; } = null!;
}