using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Public;

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
        var file = await _studyFileService
            .DownloadFile(fileId);
        
        return File(
            file.Content,
            file.ContentType,
            file.FileName);
        
    }
    
}