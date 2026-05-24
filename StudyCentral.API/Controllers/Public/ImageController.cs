using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Public;

[ApiController]
[Route("api/[controller]")]
public class ImageController : BaseController
{
    
    private readonly IBlobService _blobService;
    public ImageController(IMapper mapper, IBlobService blobService) : base(mapper)
    {
        _blobService = blobService;
    }
    
    [HttpGet]
    [Route("{blobName}")]
    public async Task<IActionResult> GetImage(string blobName)
    {
        var (stream, contentType) = await _blobService.GetFile(blobName);
        return File(stream, contentType);
    }
}