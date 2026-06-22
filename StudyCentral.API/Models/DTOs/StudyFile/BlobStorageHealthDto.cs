namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobStorageHealthDto
{
    public bool CanConnect { get; set; }
    public bool ContainerExists { get; set; }
    public bool CanCreateBlob { get; set; }
    public bool CanDeleteBlob { get; set; }
    public string? Error { get; set; }
}