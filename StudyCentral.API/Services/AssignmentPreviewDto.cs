namespace StudyCentral.API.Services;

public class AssignmentPreviewDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? Deadline { get; set; }

    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;

    public int FileCount { get; set; }
}