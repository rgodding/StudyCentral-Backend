using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Tests;

[ApiController]
[Tags("Test - Blob Storage")]
[Route("api/public/test/blob-storage")]
public class TestBlobController : BaseTestController
{
    private readonly IBlobService _blobService;

    public TestBlobController(StudyDbContext dbContext, IBlobService blobService) : base(dbContext)
    {
        _blobService = blobService;
    }

    [HttpGet("data")]
    [SwaggerOperation(
        Summary = "Get blob storage data",
        Description = "Gets a short blob storage overview."
    )]
    public async Task<IActionResult> GetBlobStorageData()
    {
        var result = await _blobService.GetBlobStorageDataAsync();
        return Ok(result);
    }

    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "Get blob storage list",
        Description = "Gets the files in blob storage."
    )]
    public async Task<IActionResult> GetBlobStorageList()
    {
        var result = await _blobService.GetBlobStorageListAsync();
        return Ok(result);
    }

    [HttpGet("health")]
    [SwaggerOperation(
        Summary = "Get blob storage health",
        Description = "Checks if blob storage is working."
    )]
    public async Task<IActionResult> GetBlobStorageHealth()
    {
        var result = await _blobService.GetBlobStorageHealthAsync();
        return Ok(result);
    }

    [HttpPost("create-test-blob")]
    [SwaggerOperation(
        Summary = "Create test blob",
        Description = "Creates a small test file in blob storage."
    )]
    public async Task<IActionResult> CreateTestBlob()
    {
        var result = await _blobService.CreateTestBlobAsync();
        return Ok(result);
    }

    [HttpDelete]
    [SwaggerOperation(
        Summary = "Wipe blob storage",
        Description = "Deletes all files from blob storage."
    )]
    public async Task<IActionResult> WipeBlobStorage()
    {
        var deletedCount = await _blobService.WipeBlobStorageAsync();
        return Ok(new { deletedCount });
    }
}
