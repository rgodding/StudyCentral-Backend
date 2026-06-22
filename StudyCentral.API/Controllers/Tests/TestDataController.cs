using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Tests;

[Tags("Test - Seed Data")]
public class TestDataController : BaseTestController
{
    private readonly IWebHostEnvironment _environment;
    private readonly ICreateTestDataService _createTestDataService;

    public TestDataController(IMapper mapper, StudyDbContext dbContext, IWebHostEnvironment environment,
        ICreateTestDataService createTestDataService) : base(mapper, dbContext)
    {
        _environment = environment;
        _createTestDataService = createTestDataService;
    }
    
    [HttpGet("reset-seed-data")]
    public async Task<IActionResult> ResetSeedData()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();

        return Ok("Seed data reset.");
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