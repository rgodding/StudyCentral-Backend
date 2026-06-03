using System.Text.RegularExpressions;

namespace StudyCentral.API.Models.Entities;

public class StudyFolder
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    
    // Course
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    // Parent Folder
    public Guid? ParentFolderId { get; set; }
    public StudyFolder? ParentFolder { get; set; }
    
    // Child Folders
    public ICollection<StudyFolder> ChildFolders { get; set; } = new List<StudyFolder>();
    
    // Files
    public ICollection<StudyFile> Files { get; set; } = new List<StudyFile>();
}