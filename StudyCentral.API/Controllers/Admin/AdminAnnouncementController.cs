using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Announcement;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Tags("Admin - Announcements")]
[Route("api/admin/announcements")]
public class AdminAnnouncementController : BaseAdminController
{
    private readonly IAnnouncementService _announcementService;

    public AdminAnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get announcements",
        Description = "Gets all announcements."
    )]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncements()
    {
        var announcements = await _announcementService.GetAll();
        return Ok(announcements);
    }

    [HttpGet("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Get announcement",
        Description = "Gets one announcement by id."
    )]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(Guid announcementId)
    {
        var announcement = await _announcementService.GetById(announcementId);
        return Ok(announcement);
    }

    [HttpPut("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Update announcement",
        Description = "Updates one announcement by id."
    )]
    public async Task<ActionResult<AnnouncementDto>> UpdateAnnouncement(Guid announcementId, [FromBody] AdminUpdateAnnouncementDto dto)
    {
        var announcement = await _announcementService.AdminUpdateAnnouncement(announcementId, dto);
        return Ok(announcement);
    }

    [HttpDelete("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Delete announcement",
        Description = "Deletes one announcement by id."
    )]
    public async Task<IActionResult> DeleteAnnouncement(Guid announcementId)
    {
        await _announcementService.Delete(announcementId);
        return NoContent();
    }

    [HttpGet("{announcementId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get announcement files",
        Description = "Gets files linked to one announcement."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(Guid announcementId)
    {
        var files = await _announcementService.GetFiles(announcementId);
        return Ok(files);
    }
}