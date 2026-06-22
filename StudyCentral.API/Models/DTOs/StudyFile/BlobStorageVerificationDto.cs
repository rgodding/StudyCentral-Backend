namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobStorageVerificationDto
{
    public int FileCount { get; set; }
    public int MissingFiles { get; set; }
    public List<BlobStorageVerificationItemDto> Files { get; set; } = [];
}