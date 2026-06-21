using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Data.Seed;

namespace StudyCentral.API.Controllers;

[ApiController]
[Route("api/public/test")]
public class TestDataController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ISchoolDemoDataSeeder _schoolDemoDataSeeder;

    public TestDataController(
        IWebHostEnvironment environment,
        ISchoolDemoDataSeeder schoolDemoDataSeeder)
    {
        _environment = environment;
        _schoolDemoDataSeeder = schoolDemoDataSeeder;
    }

    [HttpPost("create-test-data")]
    public async Task<IActionResult> CreateTestData()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var result = await _schoolDemoDataSeeder.SeedAsync();

        return Ok(result);
    }
}