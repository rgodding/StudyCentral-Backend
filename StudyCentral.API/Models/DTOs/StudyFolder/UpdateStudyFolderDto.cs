using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.StudyFolder;

public class UpdateStudyFolderDto
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; } = null!;

    public Guid? ParentFolderId { get; set; }
}