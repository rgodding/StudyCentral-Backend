using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Public;

[ApiController]
[Tags("Health")]
[AllowAnonymous]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get health",
        Description = "Checks if the API is running."
    )]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            status = "ok",
            timestamp = DateTime.UtcNow
        });
    }
}