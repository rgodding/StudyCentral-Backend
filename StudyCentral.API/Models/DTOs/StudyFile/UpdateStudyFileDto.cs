using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.StudyFile;

public class UpdateStudyFileDto
{
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = null!;

    [MaxLength(500)]
    public string? AltText { get; set; }
}