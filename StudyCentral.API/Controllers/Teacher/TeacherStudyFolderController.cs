using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Authorize(Roles = "Teacher")]
[Route("api/teacher/study-folders")]
public class TeacherStudyFolderController : BaseController
{
    private readonly IStudyFolderService _studyFolderService;

    public TeacherStudyFolderController(
        IMapper mapper,
        IStudyFolderService studyFolderService)
        : base(mapper)
    {
        _studyFolderService = studyFolderService;
    }

    // -----------------
    // Teacher Folder Endpoints
    // -----------------

    [HttpGet("courses/{courseId:guid}")]
    public async Task<ActionResult<List<StudyFolderDto>>> GetFolders(Guid courseId,
        [FromQuery] Guid? parentFolderId = null)
    {
        var folders = await _studyFolderService.GetFoldersByCourseIdAndTeacherId(
            CurrentUser.Id,
            courseId,
            parentFolderId);

        return Ok(folders);
    }

    [HttpGet("{folderId:guid}")]
    public async Task<ActionResult<StudyFolderDto>> GetFolder(Guid folderId)
    {
        var folder = await _studyFolderService
            .GetFolderByTeacherId(CurrentUser.Id, folderId);

        return Ok(folder);
    }
    
    [HttpGet("course/{courseId:guid}/content")]
    public async Task<IActionResult> GetCourseStudyFolderContent(Guid courseId)
    {
        var content = await _studyFolderService
            .GetCourseStudyFolderContentByTeacherId(CurrentUser.Id, courseId);

        return Ok(content);
    }

    [HttpPost]
    public async Task<ActionResult<StudyFolderDto>> CreateFolder(
        [FromBody] CreateStudyFolderDto dto)
    {
        var createdFolder = await _studyFolderService
            .CreateFolderByTeacherId(
                CurrentUser.Id,
                dto);

        return CreatedAtAction(
            nameof(GetFolder),
            new { folderId = createdFolder.Id },
            createdFolder);
    }

    [HttpPut("{folderId:guid}")]
    public async Task<ActionResult<StudyFolderDto>> UpdateFolder(Guid folderId, [FromBody] UpdateStudyFolderDto dto)
    {
        var folder = await _studyFolderService
            .UpdateFolderByTeacherId(CurrentUser.Id, folderId, dto);

        return Ok(folder);
    }

    [HttpDelete("{folderId:guid}")]
    public async Task<IActionResult> DeleteFolder(Guid folderId)
    {
        await _studyFolderService.DeleteFolderByTeacherId(CurrentUser.Id, folderId);
        return NoContent();
    }

    [HttpPatch("{folderId:guid}/move")]
    public async Task<ActionResult<StudyFolderDto>> MoveFolder(Guid folderId, [FromBody] MoveFolderDto dto)
    {
        var folder = await _studyFolderService
            .MoveFolderByTeacherId(CurrentUser.Id, folderId, dto.NewParentFolderId);

        return Ok(folder);
    }

    // -----------------
    // File Endpoints
    // -----------------

    [HttpGet("{folderId:guid}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(Guid folderId)
    {
        var files = await _studyFolderService
            .GetFilesByFolderIdAndTeacherId(CurrentUser.Id, folderId);

        return Ok(files);
    }

    [HttpGet("files/{fileId:guid}")]
    public async Task<ActionResult<StudyFileDto>> GetFile(Guid fileId)
    {
        var file = await _studyFolderService
            .GetFileByIdAndTeacherId(CurrentUser.Id, fileId);

        return Ok(file);
    }

    [HttpPost("{folderId:guid}/files")]
    public async Task<ActionResult<StudyFileDto>> UploadFile(
        Guid folderId,
        [FromForm] UploadFileDto dto)
    {
        var file = await _studyFolderService
            .CreateFileByTeacherId(
                CurrentUser.Id,
                folderId,
                dto);

        return CreatedAtAction(
            nameof(GetFile),
            new { fileId = file.Id },
            file);
    }

    [HttpDelete("{folderId:guid}/files/{fileId:guid}")]
    public async Task<IActionResult> RemoveFile(Guid folderId, Guid fileId)
    {
        await _studyFolderService.RemoveFileFromFolderByTeacherId(CurrentUser.Id, folderId, fileId);
        return NoContent();
    }
}