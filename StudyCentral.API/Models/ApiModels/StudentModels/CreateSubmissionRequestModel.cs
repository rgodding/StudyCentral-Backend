namespace StudyCentral.API.Models.ApiModels.StudentModels;

public class CreateSubmissionRequestModel
{
    public string? Comment { get; set; }
    public List<IFormFile>? Files { get; set; }
}