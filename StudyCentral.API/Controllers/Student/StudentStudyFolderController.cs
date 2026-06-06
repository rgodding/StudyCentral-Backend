using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Route("api/student/study-folders")]
public class StudentStudyFolderController : BaseStudentController
{
    private readonly IStudyFolderService _studyFolderService;

    public StudentStudyFolderController(IMapper mapper, IStudyFolderService studyFolderService) : base(mapper)
    {
        _studyFolderService = studyFolderService;
    }
    
    [HttpGet("courses/{courseId:guid}")]
    public async Task<ActionResult<List<StudyFolderDto>>> GetFolders(
        Guid courseId,
        [FromQuery] Guid? parentFolderId = null)
    {
        var folders = await _studyFolderService.GetFoldersByCourseIdAndStudentId(
            CurrentUser.Id,
            courseId,
            parentFolderId);
        
        return Ok(folders);
    }

    [HttpGet("{folderId:guid}")]
    public async Task<ActionResult<StudyFolderDto>> GetFolder(
        Guid folderId)
    {
        var folder = await _studyFolderService
            .GetFolderByStudentId(CurrentUser.Id, folderId);
        
        return Ok(folder);
    }

    [HttpGet("{folderId:guid}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid folderId)
    {
        var files = await _studyFolderService.GetFilesByFolderIdAndStudentId(
            CurrentUser.Id,
            folderId);
        
        return Ok(files);
    }

    [HttpGet("files/{fileId:guid}")]
    public async Task<ActionResult<StudyFileDto>> GetFile(
        Guid fileId)
    {
        var file = await _studyFolderService.GetFileByIdAndStudentId(
            CurrentUser.Id,
            fileId);
        
        return Ok(file);
    }
}