using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.Dtos.StudyFiles;

public class StudyFileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string BlobName { get; set; }
    public FileType FileType { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public string? AltText { get; set; }
    public DateTime UploadAt { get; set; }
}