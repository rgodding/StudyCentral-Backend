namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobStorageVerificationItemDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public bool Exists { get; set; }
}