using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Announcement;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Route("api/admin/announcements")]
public class AdminAnnouncementController : BaseAdminController
{
    private readonly IAnnouncementService _announcementService;

    public AdminAnnouncementController(
        IMapper mapper,
        IAnnouncementService announcementService)
        : base(mapper)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncements()
    {
        var announcements = await _announcementService.GetAll();

        return Ok(announcements);
    }

    [HttpGet("{announcementId}")]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(
        Guid announcementId)
    {
        var announcement = await _announcementService.GetById(
            announcementId);

        return Ok(announcement);
    }

    [HttpPut("{announcementId}")]
    public async Task<ActionResult<AnnouncementDto>> UpdateAnnouncement(
        Guid announcementId,
        [FromBody] AdminUpdateAnnouncementDto dto)
    {
        var announcement = await _announcementService.AdminUpdateAnnouncement(
            announcementId,
            dto);

        return Ok(announcement);
    }

    [HttpDelete("{announcementId}")]
    public async Task<IActionResult> DeleteAnnouncement(
        Guid announcementId)
    {
        await _announcementService.Delete(
            announcementId);

        return NoContent();
    }

    [HttpGet("{announcementId}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid announcementId)
    {
        var files = await _announcementService.GetFiles(
            announcementId);

        return Ok(files);
    }
}