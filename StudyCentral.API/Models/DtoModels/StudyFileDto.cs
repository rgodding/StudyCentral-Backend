using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DtoModels;

public class StudyFileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public FileType Type { get; set; }
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
}