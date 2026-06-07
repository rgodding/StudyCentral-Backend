using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Route("api/admin/study-folders")]
public class AdminStudyFolderController : BaseAdminController
{
    private readonly IStudyFolderService _studyFolderService;

    public AdminStudyFolderController(
        IMapper mapper,
        IStudyFolderService studyFolderService)
        : base(mapper)
    {
        _studyFolderService = studyFolderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudyFolderDto>>> GetStudyFolders()
    {
        var folders = await _studyFolderService.GetAll();

        return Ok(folders);
    }

    [HttpGet("{folderId}")]
    public async Task<ActionResult<StudyFolderDto>> GetStudyFolder(
        Guid folderId)
    {
        var folder = await _studyFolderService.GetById(
            folderId);

        return Ok(folder);
    }

    [HttpGet("{folderId}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid folderId)
    {
        var files = await _studyFolderService.GetFiles(
            folderId);

        return Ok(files);
    }
}