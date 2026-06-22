using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Tags("Teacher - Announcements")]
[Route("api/teacher/announcements")]
public class TeacherAnnouncementController : BaseTeacherController
{
    private readonly IAnnouncementService _announcementService;

    public TeacherAnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get announcements",
        Description = "Gets all announcements."
    )]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncements()
    {
        var announcements = await _announcementService.GetAnnouncementsByTeacherId(CurrentUserId);
        return Ok(announcements);
    }

    [HttpGet("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Get announcement by id",
        Description = "Gets one announcement by id."
    )]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncementById(Guid announcementId)
    {
        var announcement = await _announcementService.GetAnnouncementByTeacherId(CurrentUserId, announcementId);
        return Ok(announcement);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create announcement",
        Description = "Creates a new announcement."
    )]
    public async Task<ActionResult<AnnouncementDto>> CreateAnnouncement([FromBody] CreateAnnouncementDto createAnnouncementDto)
    {
        var announcement = await _announcementService.CreateAnnouncementByTeacherId(CurrentUserId, createAnnouncementDto);

        return CreatedAtAction(
            nameof(GetAnnouncementById),
            new { announcementId = announcement.Id },
            announcement);
    }

    [HttpPut("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Update announcement",
        Description = "Updates one announcement by id."
    )]
    public async Task<ActionResult<AnnouncementDto>> UpdateAnnouncement(Guid announcementId, [FromBody] UpdateAnnouncementDto updateAnnouncementDto)
    {
        var announcement = await _announcementService.UpdateAnnouncementByTeacherId(CurrentUserId, announcementId, updateAnnouncementDto);
        return Ok(announcement);
    }

    [HttpDelete("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Delete announcement",
        Description = "Deletes one announcement by id."
    )]
    public async Task<ActionResult> DeleteAnnouncement(Guid announcementId)
    {
        await _announcementService.DeleteAnnouncementByTeacherId(CurrentUserId, announcementId);
        return NoContent();
    }

    [HttpGet("courses/{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get announcements by course id",
        Description = "Gets announcements for one course."
    )]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncementsByCourseId(Guid courseId)
    {
        var announcements = await _announcementService
            .GetAnnouncementsByCourseIdAndTeacherId(
                CurrentUserId,
                courseId);
        return Ok(announcements);
    }

    [HttpGet("{announcementId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid announcementId)
    {
        var files = await _announcementService
            .GetFilesByAnnouncementIdAndTeacherId(
                CurrentUserId,
                announcementId);

        return Ok(files);
    }

    [HttpPost("{announcementId:guid}/files")]
    [SwaggerOperation(
        Summary = "Upload file",
        Description = "Uploads a file."
    )]
    public async Task<ActionResult<StudyFileDto>> UploadFile(
        Guid announcementId,
        IFormFile file)
    {
        var uploadedFile = await _announcementService
            .UploadFileToAnnouncementByTeacherId(
                CurrentUserId,
                announcementId,
                file);

        return Ok(uploadedFile);
    }

    [HttpPost("{announcementId:guid}/files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Attach file",
        Description = "Attaches a file."
    )]
    public async Task<IActionResult> AttachFile(
        Guid announcementId,
        Guid fileId)
    {
        await _announcementService
            .AttachFileToAnnouncementByTeacherId(
                CurrentUserId,
                announcementId,
                fileId);

        return NoContent();
    }

    [HttpDelete("{announcementId:guid}/files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Remove file",
        Description = "Removes one linked file."
    )]
    public async Task<IActionResult> RemoveFile(
        Guid announcementId,
        Guid fileId)
    {
        await _announcementService
            .RemoveFileFromAnnouncementByTeacherId(
                CurrentUserId,
                announcementId,
                fileId);

        return NoContent();
    }
}