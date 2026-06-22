using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Public;

[Authorize]
[ApiController]
[Tags("Files")]
[Route("api/file")]
public class FileController : BaseController
{
    private readonly IStudyFileService _studyFileService;

    public FileController(IStudyFileService studyFileService)
    {
        _studyFileService = studyFileService;
    }

    [HttpGet("{fileId:guid}/download")]
    [SwaggerOperation(
        Summary = "Download file",
        Description = "Downloads one file by id."
    )]
    public async Task<IActionResult> DownloadFile(Guid fileId)
    {
        var file = await _studyFileService
            .DownloadFile(CurrentUserId, fileId);

        return File(
            file.Content,
            file.ContentType,
            file.FileName);
    }

    [HttpGet("{fileId:guid}/preview")]
    [SwaggerOperation(
        Summary = "Preview file",
        Description = "Previews one file by id."
    )]
    public async Task<IActionResult> PreviewFile(Guid fileId)
    {
        var file = await _studyFileService
            .DownloadFile(CurrentUserId, fileId);

        Response.Headers.ContentDisposition =
            $"inline; filename=\"{file.FileName}\"";

        return File(
            file.Content,
            file.ContentType);
    }
}