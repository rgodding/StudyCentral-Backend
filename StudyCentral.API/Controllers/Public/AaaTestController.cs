using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Constants.Tests;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using StudyCentral.API.Utilities;
namespace StudyCentral.API.Controllers.Public;

[ApiController]
[Route("api/public/test")]
public class AaaTestController : BaseController
{
    private readonly StudyDbContext _dbContext;
    private readonly IAuthService _authService;
    private readonly IBlobService _blobService;
    private readonly IWebHostEnvironment _environment;
    
    public AaaTestController(IMapper mapper, StudyDbContext dbContext, IAuthService authService, IBlobService blobService, IWebHostEnvironment environment) : base(mapper)
    {
        _dbContext = dbContext;
        _authService = authService;
        _blobService = blobService;
        _environment = environment;
    } 

    [HttpGet("reset-seed-data")]
    public async Task<IActionResult> ResetSeedData()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _dbContext.Database.MigrateAsync();
        
        return Ok("Seed data reset.");
    }
    
    [HttpGet("login-admin")]
    public async Task<ActionResult<UserDto>> LoginAdmin()
    {

        var result = await _authService.Login(TestLoginDto.Admin);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }
    
    [HttpGet("login-teacher")]
    public async Task<ActionResult<UserDto>> LoginTeacher()
    {
        var result = await _authService.Login(TestLoginDto.Teacher);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    [HttpGet("login-student")]
    public async Task<ActionResult<UserDto>> LoginStudent()
    {
        var result = await _authService.Login(TestLoginDto.Student);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    [HttpGet("reset-blob-storage")]
    public async Task<IActionResult> ResetBlobStorage()
    {
        var wiped = await _blobService.Wipe();
        if(!wiped)
            return BadRequest("Failed to wipe blob storage.");
        
        var testDataPath = Path.Combine(
            _environment.WebRootPath,
            "testdata");
        
        if (!Directory.Exists(testDataPath))
            return NotFound($"Test data directory not found: {testDataPath}");
        
        var files = Directory.GetFiles(testDataPath);
        var uploadedFiles = new List<object>();

        foreach (var filePath in files)
        {
            await using var stream = System.IO.File.OpenRead(filePath);
            
            var formFile = new FormFile(
                stream,
                0,
                stream.Length,
                "File",
                Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = FileExtensionMappings.GetContentTypeFromPath(filePath)
            };
            
            var upload = await _blobService.UploadFileTest(
                formFile.FileName,
                formFile);
            
            uploadedFiles.Add(new
            {
                formFile.FileName,
                upload.BlobName,
                Size = formFile.Length,
                formFile.ContentType
            });
        }
        
        var blobCount = await _blobService.GetBlobCount();
        
        return Ok(new
        {
            Message = "Blob storage reset and test files uploaded.",
            UploadedFiles = uploadedFiles,
            BlobCount = blobCount
        });
        
    }
    
    [HttpGet("verify-study-files")]
    public async Task<IActionResult> VerifyStudyFiles()
    {
        var files = await _dbContext.StudyFiles
            .ToListAsync();

        var results = new List<object>();

        foreach (var file in files)
        {
            var exists = await _blobService.FileExists(file.BlobName);

            results.Add(new
            {
                file.Id,
                file.FileName,
                file.BlobName,
                file.ContentType,
                file.Size,
                Exists = exists
            });
        }

        var missingFiles = results
            .Count(r => !(bool)r.GetType().GetProperty("Exists")!.GetValue(r)!);

        if (missingFiles > 0)
        {
            return BadRequest(new
            {
                FileCount = files.Count,
                MissingFiles = missingFiles,
                Files = results
            });
        }

        return Ok(new
        {
            FileCount = files.Count,
            MissingFiles = 0,
            Files = results
        });
    }

    
    
    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}