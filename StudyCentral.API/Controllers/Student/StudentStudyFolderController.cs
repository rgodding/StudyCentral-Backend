using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Tags("Student - Study Folders")]
[Route("api/student/study-folders")]
public class StudentStudyFolderController : BaseStudentController
{
    private readonly IStudyFolderService _studyFolderService;

    public StudentStudyFolderController(IStudyFolderService studyFolderService)
    {
        _studyFolderService = studyFolderService;
    }

    [HttpGet("courses/{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get folders",
        Description = "Gets study folders."
    )]
    public async Task<ActionResult<List<StudyFolderDto>>> GetFolders(
        Guid courseId,
        [FromQuery] Guid? parentFolderId = null)
    {
        var folders = await _studyFolderService.GetFoldersByCourseIdAndStudentId(
            CurrentUserId,
            courseId,
            parentFolderId);

        return Ok(folders);
    }

    [HttpGet("course/{courseId:guid}/content")]
    [SwaggerOperation(
        Summary = "Get course study folder content",
        Description = "Gets study folder content for one course."
    )]
    public async Task<IActionResult> GetCourseStudyFolderContent(Guid courseId)
    {
        var content = await _studyFolderService
            .GetCourseStudyFolderContentByStudentId(CurrentUserId, courseId);

        return Ok(content);
    }

    [HttpGet("{folderId:guid}")]
    [SwaggerOperation(
        Summary = "Get folder",
        Description = "Gets one study folder by id."
    )]
    public async Task<ActionResult<StudyFolderDto>> GetFolder(
        Guid folderId)
    {
        var folder = await _studyFolderService
            .GetFolderByStudentId(CurrentUserId, folderId);

        return Ok(folder);
    }

    [HttpGet("{folderId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid folderId)
    {
        var files = await _studyFolderService.GetFilesByFolderIdAndStudentId(
            CurrentUserId,
            folderId);

        return Ok(files);
    }

    [HttpGet("files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Get file",
        Description = "Gets one file by id."
    )]
    public async Task<ActionResult<StudyFileDto>> GetFile(
        Guid fileId)
    {
        var file = await _studyFolderService.GetFileByIdAndStudentId(
            CurrentUserId,
            fileId);

        return Ok(file);
    }
}