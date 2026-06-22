using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Tests;

[ApiController]
[Tags("Test - Seed Data")]
[Route("api/public/test/seed-data")]
public class TestDataController : BaseTestController
{
    private readonly IWebHostEnvironment _environment;
    private readonly ICreateTestDataService _createTestDataService;

    public TestDataController(StudyDbContext dbContext, IWebHostEnvironment environment,
        ICreateTestDataService createTestDataService) : base(dbContext)
    {
        _environment = environment;
        _createTestDataService = createTestDataService;
    }

    [HttpGet("reset-seed-data")]
    [SwaggerOperation(
        Summary = "Reset seed data",
        Description = "Resets the seed database."
    )]
    public async Task<IActionResult> ResetSeedData()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();

        return Ok("Seed data reset.");
    }

    [HttpPost("create-test-data")]
    [SwaggerOperation(
        Summary = "Create test data",
        Description = "Creates development test data."
    )]
    public async Task<IActionResult> CreateTestData()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var result = await _createTestDataService.CreateBig();

        return Ok(result);
    }

    [HttpPost("create-course-test-data")]
    [SwaggerOperation(
        Summary = "Create course test data",
        Description = "Creates course test data."
    )]
    public async Task<IActionResult> CreateCourseTestData()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var result = await _createTestDataService.CreateCourse();

        return Ok(result);
    }
}
