namespace StudyCentral.API.Models.DTOs.StudyFile;

public class BlobStorageDataDto
{
    public string ContainerName { get; set; } = string.Empty;
    public int BlobCount { get; set; }
    public long TotalBytes { get; set; }
    public double TotalMb { get; set; }
    public BlobStorageItemDto? LargestBlob { get; set; }
    public BlobStorageItemDto? NewestBlob { get; set; }
    public BlobStorageItemDto? OldestBlob { get; set; }
}