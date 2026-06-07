using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Models.ApiModels.Submission;

public class UploadSubmissionFileRequest
{
    [FromForm(Name = "file")]
    public IFormFile File { get; set; } = null!;
}