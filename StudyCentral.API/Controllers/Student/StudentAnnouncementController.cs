using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Tags("Student - Announcements")]
[Route("api/student/announcements")]
public class StudentAnnouncementController : BaseStudentController
{
    private readonly IAnnouncementService _announcementService;

    public StudentAnnouncementController(IAnnouncementService announcementService)
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
        var announcements = await _announcementService
            .GetAnnouncementsByStudentId(CurrentUserId);

        return Ok(announcements);
    }

    [HttpGet("courses/{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get announcements by course id",
        Description = "Gets announcements for one course."
    )]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncementsByCourseId(
        Guid courseId)
    {
        var announcements = await _announcementService
            .GetAnnouncementsByCourseIdAndStudentId(
                CurrentUserId,
                courseId);

        return Ok(announcements);
    }

    [HttpGet("{announcementId:guid}")]
    [SwaggerOperation(
        Summary = "Get announcement",
        Description = "Gets one announcement by id."
    )]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(
        Guid announcementId)
    {
        var announcement = await _announcementService
            .GetAnnouncementByStudentId(
                CurrentUserId,
                announcementId);

        return Ok(announcement);
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
            .GetFilesByAnnouncementIdAndStudentId(
                CurrentUserId,
                announcementId);

        return Ok(files);
    }
}