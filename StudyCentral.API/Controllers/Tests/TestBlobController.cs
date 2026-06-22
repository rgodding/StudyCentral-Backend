using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Tests;

[Tags("Test - Blob Storage")]
[Route("api/public/test/blob-storage")]
public class TestBlobController : BaseTestController
{
    private readonly IBlobService _blobService;
    
    public TestBlobController(IMapper mapper, StudyDbContext dbContext, IBlobService blobService) : base(mapper, dbContext)
    {
        _blobService = blobService;
    }

    [HttpGet("data")]
    [SwaggerOperation(
        Summary = "Get blob storage overview",
        Description = "Returns diagnostic information about the blob container, including blob count, total storage size, largest blob, newest blob, and oldest blob."
    )]
    [ProducesResponseType(typeof(BlobStorageDataDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlobStorageData()
    {
        var result = await _blobService.GetBlobStorageDataAsync();
        return Ok(result);
    }

    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List all blobs",
        Description = "Returns all blobs currently stored in the blob container, including name, content type, file size, file extension, and last modified date."
    )]
    [ProducesResponseType(typeof(List<BlobStorageItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlobStorageList()
    {
        var result = await _blobService.GetBlobStorageListAsync();
        return Ok(result);
    }

    [HttpGet("health")]
    [SwaggerOperation(
        Summary = "Check blob storage health",
        Description = "Tests whether the API can connect to blob storage, create a temporary blob, and delete it again. Useful for checking Azurite or Azure Blob Storage configuration."
    )]
    [ProducesResponseType(typeof(BlobStorageHealthDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlobStorageHealth()
    {
        var result = await _blobService.GetBlobStorageHealthAsync();
        return Ok(result);
    }

    [HttpPost("create-test-blob")]
    [SwaggerOperation(
        Summary = "Create a test blob",
        Description = "Creates a small text blob in the blob container. Useful for testing whether blob upload works without using the frontend."
    )]
    [ProducesResponseType(typeof(BlobStorageItemDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTestBlob()
    {
        var result = await _blobService.CreateTestBlobAsync();
        return Ok(result);
    }

    [HttpDelete]
    [SwaggerOperation(
        Summary = "Wipe blob storage",
        Description = "Deletes every blob in the configured blob container. This is intended for local development and test cleanup only."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> WipeBlobStorage()
    {
        var deletedCount = await _blobService.WipeBlobStorageAsync();
        return Ok(new { deletedCount });
    }
}