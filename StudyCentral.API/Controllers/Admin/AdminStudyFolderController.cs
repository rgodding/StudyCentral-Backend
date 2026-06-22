using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.StudyFolder;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Tags("Admin - Study Folders")]
[Route("api/admin/study-folders")]
public class AdminStudyFolderController : BaseAdminController
{
    private readonly IStudyFolderService _studyFolderService;

    public AdminStudyFolderController(IStudyFolderService studyFolderService)
    {
        _studyFolderService = studyFolderService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get study folders",
        Description = "Gets all study folders."
    )]
    public async Task<ActionResult<List<StudyFolderDto>>> GetStudyFolders()
    {
        var folders = await _studyFolderService.GetAll();

        return Ok(folders);
    }

    [HttpGet("{folderId:guid}")]
    [SwaggerOperation(
        Summary = "Get study folder",
        Description = "Gets one study folder by id."
    )]
    public async Task<ActionResult<StudyFolderDto>> GetStudyFolder(Guid folderId)
    {
        var folder = await _studyFolderService.GetById(folderId);
        return Ok(folder);
    }

    [HttpGet("{folderId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(Guid folderId)
    {
        var files = await _studyFolderService.GetFiles(folderId);
        return Ok(files);
    }
}
