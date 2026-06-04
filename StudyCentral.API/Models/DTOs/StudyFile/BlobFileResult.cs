namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobFileResult
{
    public Stream Content { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string FileName { get; set; } = null!;
}