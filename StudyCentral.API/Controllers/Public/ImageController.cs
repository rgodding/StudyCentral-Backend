using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Public;

[ApiController]
[Tags("Images")]
[Route("api/image")]
public class ImageController : BaseController
{

    private readonly IBlobService _blobService;
    public ImageController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpGet]
    [Route("{blobName}")]
    [SwaggerOperation(
        Summary = "Get image",
        Description = "Gets one image by blob name."
    )]
    public async Task<IActionResult> GetImage(string blobName)
    {
        var file = await _blobService.GetFile(blobName);

        return File(file.Content, file.ContentType);
    }
}