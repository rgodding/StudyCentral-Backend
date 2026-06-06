using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Announcement;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/announcements")]
public class TeacherAnnouncementController : BaseTeacherController
{
    private readonly IAnnouncementService _announcementService;
    
    public TeacherAnnouncementController(IMapper mapper, IAnnouncementService announcementService) : base(mapper)
    {
        _announcementService = announcementService;
    }
    
    [HttpGet]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncements()
    {
        var announcements = await _announcementService.GetAnnouncementsByTeacherId(CurrentUser.Id);
        return Ok(announcements);
    }
    
    [HttpGet("{announcementId:guid}")]
    public async Task<ActionResult<AnnouncementDto>> GetAnnouncementById(Guid announcementId)
    {
        var announcement = await _announcementService.GetAnnouncementByTeacherId(CurrentUser.Id, announcementId);
        return Ok(announcement);
    }
    
    [HttpPost]
    public async Task<ActionResult<AnnouncementDto>> CreateAnnouncement([FromBody] CreateAnnouncementDto createAnnouncementDto)
    {
        var announcement = await _announcementService.CreateAnnouncementByTeacherId(CurrentUser.Id, createAnnouncementDto);
        
        return CreatedAtAction(
            nameof(GetAnnouncementById),
            new { announcementId = announcement.Id },
            announcement);
    }
    
    [HttpPut("{announcementId:guid}")]
    public async Task<ActionResult<AnnouncementDto>> UpdateAnnouncement(Guid announcementId, [FromBody] UpdateAnnouncementDto updateAnnouncementDto)
    {
        var announcement = await _announcementService.UpdateAnnouncementByTeacherId(CurrentUser.Id, announcementId, updateAnnouncementDto);
        return Ok(announcement);
    }
    
    [HttpDelete("{announcementId:guid}")]
    public async Task<ActionResult> DeleteAnnouncement(Guid announcementId)
    {
        await _announcementService.DeleteAnnouncementByTeacherId(CurrentUser.Id, announcementId);
        return NoContent();
    }

    [HttpGet("courses/{courseId:guid}")]
    public async Task<ActionResult<List<AnnouncementDto>>> GetAnnouncementsByCourseId(Guid courseId)
    {
        var announcements = await _announcementService
            .GetAnnouncementsByCourseIdAndTeacherId(
                CurrentUser.Id,
                courseId);
        return Ok(announcements);
    }
    
    [HttpGet("{announcementId:guid}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid announcementId)
    {
        var files = await _announcementService
            .GetFilesByAnnouncementIdAndTeacherId(
                CurrentUser.Id,
                announcementId);

        return Ok(files);
    }
    
    [HttpPost("{announcementId:guid}/files")]
    public async Task<ActionResult<StudyFileDto>> UploadFile(
        Guid announcementId,
        IFormFile file)
    {
        var uploadedFile = await _announcementService
            .UploadFileToAnnouncementByTeacherId(
                CurrentUser.Id,
                announcementId,
                file);

        return Ok(uploadedFile);
    }
    
    [HttpPost("{announcementId:guid}/files/{fileId:guid}")]
    public async Task<IActionResult> AttachFile(
        Guid announcementId,
        Guid fileId)
    {
        await _announcementService
            .AttachFileToAnnouncementByTeacherId(
                CurrentUser.Id,
                announcementId,
                fileId);

        return NoContent();
    }
    
    [HttpDelete("{announcementId:guid}/files/{fileId:guid}")]
    public async Task<IActionResult> RemoveFile(
        Guid announcementId,
        Guid fileId)
    {
        await _announcementService
            .RemoveFileFromAnnouncementByTeacherId(
                CurrentUser.Id,
                announcementId,
                fileId);

        return NoContent();
    }
}