using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/assignments")]
public class TeacherAssignmentController : BaseTeacherController
{
    private readonly IAssignmentService _assignmentService;

    public TeacherAssignmentController(
        IMapper mapper,
        IAssignmentService assignmentService) : base(mapper)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _assignmentService
            .GetAssignmentsByTeacherId(CurrentUser.Id);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}")]
    public async Task<IActionResult> GetAssignment(
        Guid assignmentId)
    {
        var assignment = await _assignmentService
            .GetAssignmentByTeacherId(
                CurrentUser.Id,
                assignmentId);

        return Ok(assignment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignment(
        [FromBody] CreateAssignmentDto dto)
    {
        var createdAssignment = await _assignmentService
            .CreateAssignmentByTeacherId(
                CurrentUser.Id,
                dto);

        return CreatedAtAction(
            nameof(GetAssignment),
            new { assignmentId = createdAssignment.Id },
            createdAssignment);
    }

    [HttpPut("{assignmentId:guid}")]
    public async Task<IActionResult> UpdateAssignment(
        Guid assignmentId,
        [FromBody] UpdateAssignmentDto dto)
    {
        var updatedAssignment = await _assignmentService
            .UpdateAssignmentByTeacherId(
                CurrentUser.Id,
                assignmentId,
                dto);

        return Ok(updatedAssignment);
    }

    [HttpDelete("{assignmentId:guid}")]
    public async Task<IActionResult> DeleteAssignment(
        Guid assignmentId)
    {
        await _assignmentService.DeleteAssignmentByTeacherId(CurrentUser.Id, assignmentId);
        return NoContent();
    }

    [HttpGet("course/{courseId:guid}")]
    public async Task<IActionResult> GetAssignmentsByCourse(
        Guid courseId)
    {
        var assignments = await _assignmentService
            .GetAssignmentsByCourseIdAndTeacherId(
                CurrentUser.Id,
                courseId);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}/files")]
    public async Task<IActionResult> GetFiles(
        Guid assignmentId)
    {
        var files = await _assignmentService
            .GetFilesByAssignmentIdAndTeacherId(
                CurrentUser.Id,
                assignmentId);

        return Ok(files);
    }

    [HttpPost("{assignmentId:guid}/files")]
    public async Task<IActionResult> UploadFile(
        Guid assignmentId,
        [FromForm] UploadFileDto file)
    {
        var uploadedFile = await _assignmentService
            .UploadFileToAssignmentByTeacherId(
                CurrentUser.Id,
                assignmentId,
                file.File);

        return CreatedAtAction(
            nameof(GetFiles),
            new { assignmentId },
            uploadedFile);
    }

    [HttpDelete("{assignmentId:guid}/files/{fileId:guid}")]
    public async Task<IActionResult> DeleteFile(
        Guid assignmentId,
        Guid fileId)
    {
        await _assignmentService
            .DeleteFileFromAssignmentByTeacherId(
                CurrentUser.Id,
                assignmentId,
                fileId);

        return NoContent();
    }

    [HttpPost("{assignmentId:guid}/attachments/{fileId:guid}")]
    public async Task<IActionResult> AttachFile(
        Guid assignmentId,
        Guid fileId)
    {
        await _assignmentService.AttachFileToAssignmentByTeacherId(
            CurrentUser.Id,
            assignmentId,
            fileId);

        return NoContent();
    }

    [HttpDelete("{assignmentId:guid}/attachments/{fileId:guid}")]
    public async Task<IActionResult> DetachFile(
        Guid assignmentId,
        Guid fileId)
    {
        await _assignmentService.DetachFileFromAssignmentByTeacherId(
            CurrentUser.Id,
            assignmentId,
            fileId);

        return NoContent();
    }
}