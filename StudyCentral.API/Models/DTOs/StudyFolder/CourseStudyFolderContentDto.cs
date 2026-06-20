using StudyCentral.API.Models.DTOs.StudyFile;

namespace StudyCentral.API.Models.DTOs.StudyFolder;

public class CourseStudyFolderContentDto
{
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;

    public List<StudyFolderDto> Folders { get; set; } = [];
    public List<StudyFileDto> Files { get; set; } = [];
}