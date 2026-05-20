using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.PublicController;

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
    [Route("{filename}")]
    public async Task<IActionResult> GetImage(string filename)
    {
        var (image, fileType) = await _blobService.GetImage(filename);
        return File(image, fileType);
    }
}