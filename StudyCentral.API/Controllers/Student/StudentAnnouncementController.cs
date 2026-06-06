using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Student;

[Route("api/student/announcements")]
public class StudentAnnouncementController : BaseStudentController
{
    private readonly IAnnouncementService _announcementService;

    public StudentAnnouncementController(
        IMapper mapper,
        IAnnouncementService announcementService)
        : base(mapper)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncements()
    {
        var announcements = await _announcementService
            .GetAnnouncementsByStudentId(CurrentUser.Id);

        return Ok(announcements);
    }

    [HttpGet("courses/{courseId:guid}")]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncementsByCourseId(
        Guid courseId)
    {
        var announcements = await _announcementService
            .GetAnnouncementsByCourseIdAndStudentId(
                CurrentUser.Id,
                courseId);

        return Ok(announcements);
    }

    [HttpGet("{announcementId:guid}")]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncement(
        Guid announcementId)
    {
        var announcement = await _announcementService
            .GetAnnouncementByStudentId(
                CurrentUser.Id,
                announcementId);

        return Ok(announcement);
    }

    [HttpGet("{announcementId:guid}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid announcementId)
    {
        var files = await _announcementService
            .GetFilesByAnnouncementIdAndStudentId(
                CurrentUser.Id,
                announcementId);

        return Ok(files);
    }
}