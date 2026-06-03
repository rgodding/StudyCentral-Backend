namespace StudyCentral.API.Models.DTOs.StudyFolder;

public class StudyFolderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = null!;
    
    public Guid? ParentFolderId { get; set; }
    
    public int ChildFolderCount { get; set; }
    public int FileCount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
}