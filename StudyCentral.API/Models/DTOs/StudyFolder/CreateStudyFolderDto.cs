using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.StudyFolder;

public class CreateStudyFolderDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    public Guid CourseId { get; set; }

    public Guid? ParentFolderId { get; set; }
}