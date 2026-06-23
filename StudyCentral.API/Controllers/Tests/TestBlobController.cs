using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Tests;

[ApiController]
[Tags("Test - Blob Storage")]
[Route("api/public/test/blob-storage")]
public class TestBlobController : BaseTestController
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = CreateContentTypeProvider();

    private readonly StudyDbContext _dbContext;
    private readonly IBlobService _blobService;
    private readonly IWebHostEnvironment _environment;

    public TestBlobController(
        StudyDbContext dbContext,
        IBlobService blobService,
        IWebHostEnvironment environment) : base(dbContext)
    {
        _dbContext = dbContext;
        _blobService = blobService;
        _environment = environment;
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

    [HttpPost("assign-testdata-to-assignments")]
    [SwaggerOperation(
        Summary = "Assign testdata files to assignments",
        Description = "Uploads every file from testdata and assigns the resulting StudyFile rows to assignments."
    )]
    public async Task<IActionResult> AssignTestDataFilesToAssignments()
    {
        var testDataPath = ResolveTestDataPath();

        if (!Directory.Exists(testDataPath))
            return NotFound($"Test data directory not found: {testDataPath}");

        var filePaths = Directory
            .GetFiles(testDataPath)
            .OrderBy(Path.GetFileName)
            .ToList();

        if (filePaths.Count == 0)
            return NotFound($"No files found in test data directory: {testDataPath}");

        var assignments = await _dbContext.Assignments
            .Include(a => a.Course)
            .OrderBy(a => a.Course.Name)
            .ThenBy(a => a.CreatedAt)
            .ThenBy(a => a.Name)
            .ToListAsync();

        if (assignments.Count == 0)
            return BadRequest("No assignments found. Create assignment test data first.");

        var fallbackUploaderId = await GetFallbackUploaderId();

        if (fallbackUploaderId == null)
            return BadRequest("No users found. Cannot assign UploadedById for test files.");

        var results = new List<object>();

        for (var index = 0; index < filePaths.Count; index++)
        {
            var filePath = filePaths[index];
            var assignment = assignments[index % assignments.Count];

            var fileName = Path.GetFileName(filePath);
            var safeBlobFileName = fileName.Replace(" ", "_");
            var blobName = $"assignments-testdata/{safeBlobFileName}";
            var contentType = GetContentType(filePath);
            var fileInfo = new FileInfo(filePath);

            await using var stream = System.IO.File.OpenRead(filePath);

            var formFile = new FormFile(
                stream,
                0,
                stream.Length,
                "file",
                fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            var blobAlreadyExisted = await _blobService.FileExists(blobName);

            if (!blobAlreadyExisted)
            {
                await _blobService.UploadFileTest(
                    formFile.FileName,
                    formFile,
                    blobName);
            }

            var uploaderId = assignment.Course.TeacherId ?? fallbackUploaderId.Value;

            var studyFile = await _dbContext.StudyFiles
                .FirstOrDefaultAsync(f => f.BlobName == blobName);

            if (studyFile == null)
            {
                studyFile = new StudyFile
                {
                    FileName = fileName,
                    BlobName = blobName,
                    ContentType = contentType,
                    Size = fileInfo.Length,
                    FileType = GetFileType(contentType, fileName),
                    UploadedById = uploaderId,
                    AssignmentId = assignment.Id,
                    StudyFolderId = null,
                    AnnouncementId = null,
                    SubmissionId = null,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.StudyFiles.Add(studyFile);
            }
            else
            {
                studyFile.FileName = fileName;
                studyFile.ContentType = contentType;
                studyFile.Size = fileInfo.Length;
                studyFile.FileType = GetFileType(contentType, fileName);
                studyFile.UploadedById = uploaderId;
                studyFile.AssignmentId = assignment.Id;
                studyFile.StudyFolderId = null;
                studyFile.AnnouncementId = null;
                studyFile.SubmissionId = null;
                studyFile.UpdatedAt = DateTime.UtcNow;
            }

            results.Add(new
            {
                FileName = fileName,
                BlobName = blobName,
                ContentType = contentType,
                Size = fileInfo.Length,
                AssignmentId = assignment.Id,
                AssignmentName = assignment.Name,
                CourseId = assignment.CourseId,
                CourseName = assignment.Course.Name,
                BlobAlreadyExisted = blobAlreadyExisted
            });
        }

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            Message = "Testdata files assigned to assignments.",
            TestDataPath = testDataPath,
            FileCount = results.Count,
            AssignmentCount = assignments.Count,
            Files = results
        });
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

    private string ResolveTestDataPath()
    {
        var webRootPath = _environment.WebRootPath
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var webRootTestDataPath = Path.Combine(webRootPath, "testdata");

        if (Directory.Exists(webRootTestDataPath))
            return webRootTestDataPath;

        return Path.Combine(Directory.GetCurrentDirectory(), "testdata");
    }

    private async Task<Guid?> GetFallbackUploaderId()
    {
        var teacherId = await _dbContext.Users
            .Where(u => u.Role == UserRole.Teacher)
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync();

        if (teacherId != null)
            return teacherId;

        var adminId = await _dbContext.Users
            .Where(u => u.Role == UserRole.Admin)
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync();

        if (adminId != null)
            return adminId;

        return await _dbContext.Users
            .Select(u => (Guid?)u.Id)
            .FirstOrDefaultAsync();
    }

    private static string GetContentType(string filePath)
    {
        if (ContentTypeProvider.TryGetContentType(filePath, out var contentType))
            return contentType;

        return "application/octet-stream";
    }

    private static FileType GetFileType(string contentType, string fileName)
    {
        var normalizedContentType = contentType.ToLowerInvariant();
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (normalizedContentType.StartsWith("image/"))
            return FileType.Image;

        if (normalizedContentType.StartsWith("video/") ||
            extension is ".mp4" or ".mov" or ".webm")
            return FileType.Video;

        if (normalizedContentType.StartsWith("audio/") ||
            extension is ".mp3" or ".wav" or ".ogg")
            return FileType.Audio;

        if (normalizedContentType == "application/pdf" || extension == ".pdf")
            return FileType.Pdf;

        if (extension is ".doc" or ".docx" or ".odt" or ".txt" or ".md" or ".csv" or ".json" or ".xml")
            return FileType.Document;

        return FileType.Other;
    }

    private static FileExtensionContentTypeProvider CreateContentTypeProvider()
    {
        var provider = new FileExtensionContentTypeProvider();

        provider.Mappings[".odt"] = "application/vnd.oasis.opendocument.text";
        provider.Mappings[".mp3"] = "audio/mpeg";
        provider.Mappings[".wav"] = "audio/wav";
        provider.Mappings[".ogg"] = "audio/ogg";
        provider.Mappings[".mp4"] = "video/mp4";
        provider.Mappings[".mov"] = "video/quicktime";
        provider.Mappings[".webm"] = "video/webm";

        return provider;
    }
}