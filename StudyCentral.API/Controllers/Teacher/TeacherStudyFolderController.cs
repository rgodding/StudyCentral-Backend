using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Tags("Teacher - Study Folders")]
[Authorize(Roles = "Teacher")]
[Route("api/teacher/study-folders")]
public class TeacherStudyFolderController : BaseController
{
    private readonly IStudyFolderService _studyFolderService;

    public TeacherStudyFolderController(IStudyFolderService studyFolderService)
    {
        _studyFolderService = studyFolderService;
    }

    // -----------------
    // Teacher Folder Endpoints
    // -----------------

    [HttpGet("courses/{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get folders",
        Description = "Gets study folders."
    )]
    public async Task<ActionResult<List<StudyFolderDto>>> GetFolders(Guid courseId,
        [FromQuery] Guid? parentFolderId = null)
    {
        var folders = await _studyFolderService.GetFoldersByCourseIdAndTeacherId(
            CurrentUserId,
            courseId,
            parentFolderId);

        return Ok(folders);
    }

    [HttpGet("{folderId:guid}")]
    [SwaggerOperation(
        Summary = "Get folder",
        Description = "Gets one study folder by id."
    )]
    public async Task<ActionResult<StudyFolderDto>> GetFolder(Guid folderId)
    {
        var folder = await _studyFolderService
            .GetFolderByTeacherId(CurrentUserId, folderId);

        return Ok(folder);
    }

    [HttpGet("course/{courseId:guid}/content")]
    [SwaggerOperation(
        Summary = "Get course study folder content",
        Description = "Gets study folder content for one course."
    )]
    public async Task<IActionResult> GetCourseStudyFolderContent(Guid courseId)
    {
        var content = await _studyFolderService
            .GetCourseStudyFolderContentByTeacherId(CurrentUserId, courseId);

        return Ok(content);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create folder",
        Description = "Creates a new study folder."
    )]
    public async Task<ActionResult<StudyFolderDto>> CreateFolder(
        [FromBody] CreateStudyFolderDto dto)
    {
        var createdFolder = await _studyFolderService
            .CreateFolderByTeacherId(
                CurrentUserId,
                dto);

        return CreatedAtAction(
            nameof(GetFolder),
            new { folderId = createdFolder.Id },
            createdFolder);
    }

    [HttpPut("{folderId:guid}")]
    [SwaggerOperation(
        Summary = "Update folder",
        Description = "Updates one study folder by id."
    )]
    public async Task<ActionResult<StudyFolderDto>> UpdateFolder(Guid folderId, [FromBody] UpdateStudyFolderDto dto)
    {
        var folder = await _studyFolderService
            .UpdateFolderByTeacherId(CurrentUserId, folderId, dto);

        return Ok(folder);
    }

    [HttpDelete("{folderId:guid}")]
    [SwaggerOperation(
        Summary = "Delete folder",
        Description = "Deletes one study folder by id."
    )]
    public async Task<IActionResult> DeleteFolder(Guid folderId)
    {
        await _studyFolderService.DeleteFolderByTeacherId(CurrentUserId, folderId);
        return NoContent();
    }

    [HttpPatch("{folderId:guid}/move")]
    [SwaggerOperation(
        Summary = "Move folder",
        Description = "Moves one study folder."
    )]
    public async Task<ActionResult<StudyFolderDto>> MoveFolder(Guid folderId, [FromBody] MoveFolderDto dto)
    {
        var folder = await _studyFolderService
            .MoveFolderByTeacherId(CurrentUserId, folderId, dto.NewParentFolderId);

        return Ok(folder);
    }

    // -----------------
    // File Endpoints
    // -----------------

    [HttpGet("{folderId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(Guid folderId)
    {
        var files = await _studyFolderService
            .GetFilesByFolderIdAndTeacherId(CurrentUserId, folderId);

        return Ok(files);
    }

    [HttpGet("files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Get file",
        Description = "Gets one file by id."
    )]
    public async Task<ActionResult<StudyFileDto>> GetFile(Guid fileId)
    {
        var file = await _studyFolderService
            .GetFileByIdAndTeacherId(CurrentUserId, fileId);

        return Ok(file);
    }

    [HttpPost("{folderId:guid}/files")]
    [SwaggerOperation(
        Summary = "Upload file",
        Description = "Uploads a file."
    )]
    public async Task<ActionResult<StudyFileDto>> UploadFile(
        Guid folderId,
        [FromForm] UploadFileDto dto)
    {
        var file = await _studyFolderService
            .CreateFileByTeacherId(
                CurrentUserId,
                folderId,
                dto);

        return CreatedAtAction(
            nameof(GetFile),
            new { fileId = file.Id },
            file);
    }

    [HttpDelete("{folderId:guid}/files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Remove file",
        Description = "Removes one linked file."
    )]
    public async Task<IActionResult> RemoveFile(Guid folderId, Guid fileId)
    {
        await _studyFolderService.RemoveFileFromFolderByTeacherId(CurrentUserId, folderId, fileId);
        return NoContent();
    }
}