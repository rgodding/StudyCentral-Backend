using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers;

[ApiController]
[Route("api/public/test")]
public class TestDataController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly ICreateTestDataService _createTestDataService;

    public TestDataController(
        IWebHostEnvironment environment,
        ICreateTestDataService createTestDataService)
    {
        _environment = environment;
        _createTestDataService = createTestDataService;
    }

    [HttpPost("create-test-data")]
    public async Task<IActionResult> CreateTestData()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var result = await _createTestDataService.CreateBig();

        return Ok(result);
    }

    [HttpPost("create-course-test-data")]
    public async Task<IActionResult> CreateCourseTestData()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var result = await _createTestDataService.CreateCourse();

        return Ok(result);
    }
}
