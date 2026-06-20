using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Public;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FileController : BaseController
{
    private readonly IStudyFileService _studyFileService;

    public FileController(IMapper mapper, IStudyFileService studyFileService) : base(mapper)
    {
        _studyFileService = studyFileService;
    }

    [HttpGet("{fileId:guid}/download")]
    public async Task<IActionResult> DownloadFile(Guid fileId)
    {
        Console.WriteLine("current user: " + CurrentUser.Id);
        Console.WriteLine("file id: " + fileId);
        Console.WriteLine("User Role: " + CurrentUser.Role + "\n");
        var file = await _studyFileService
            .DownloadFile(CurrentUser.Id, fileId);

        return File(
            file.Content,
            file.ContentType,
            file.FileName);
    }
}