namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobUploadResult
{
    public string FileName { get; set; } = null!;
    public string BlobName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}