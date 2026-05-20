using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.PublicController;

[ApiController]
[Route("api/[controller]")]
public class HealthController : BaseController
{
    public HealthController(IMapper mapper) : base(mapper)
    {
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow
        });
    }
}