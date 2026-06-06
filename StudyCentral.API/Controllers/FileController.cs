using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : BaseController
{
    private readonly IStudyFileService _studyFileService;
    
    public FileController(IMapper mapper, IStudyFileService studyFileService) : base(mapper)
    {
        _studyFileService = studyFileService;
    }
    
    [HttpGet("{fileId:guid}/url")]
    public async Task<IActionResult> GetFileUrl(Guid fileId)
    {
        var url = await _studyFileService.GetFileUrl(fileId);
        return Ok(url);
    }
    
}