using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Tags("Teacher - Assignments")]
[Route("api/teacher/assignments")]
public class TeacherAssignmentController : BaseTeacherController
{
    private readonly IAssignmentService _assignmentService;

    public TeacherAssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get assignments",
        Description = "Gets all assignments."
    )]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _assignmentService
            .GetAssignmentsByTeacherId(CurrentUserId);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Get assignment",
        Description = "Gets one assignment by id."
    )]
    public async Task<IActionResult> GetAssignment(
        Guid assignmentId)
    {
        var assignment = await _assignmentService
            .GetAssignmentByTeacherId(
                CurrentUserId,
                assignmentId);

        return Ok(assignment);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create assignment",
        Description = "Creates a new assignment."
    )]
    public async Task<IActionResult> CreateAssignment(
        [FromBody] CreateAssignmentDto dto)
    {
        var createdAssignment = await _assignmentService
            .CreateAssignmentByTeacherId(
                CurrentUserId,
                dto);

        return CreatedAtAction(
            nameof(GetAssignment),
            new { assignmentId = createdAssignment.Id },
            createdAssignment);
    }

    [HttpPut("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Update assignment",
        Description = "Updates one assignment by id."
    )]
    public async Task<IActionResult> UpdateAssignment(
        Guid assignmentId,
        [FromBody] UpdateAssignmentDto dto)
    {
        var updatedAssignment = await _assignmentService
            .UpdateAssignmentByTeacherId(
                CurrentUserId,
                assignmentId,
                dto);

        return Ok(updatedAssignment);
    }

    [HttpDelete("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Delete assignment",
        Description = "Deletes one assignment by id."
    )]
    public async Task<IActionResult> DeleteAssignment(
        Guid assignmentId)
    {
        await _assignmentService.DeleteAssignmentByTeacherId(CurrentUserId, assignmentId);
        return NoContent();
    }

    [HttpGet("course/{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get assignments by course",
        Description = "Gets assignments for one course."
    )]
    public async Task<IActionResult> GetAssignmentsByCourse(
        Guid courseId)
    {
        var assignments = await _assignmentService
            .GetAssignmentsByCourseIdAndTeacherId(
                CurrentUserId,
                courseId);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<IActionResult> GetFiles(
        Guid assignmentId)
    {
        var files = await _assignmentService
            .GetFilesByAssignmentIdAndTeacherId(
                CurrentUserId,
                assignmentId);

        return Ok(files);
    }

    [HttpPost("{assignmentId:guid}/files")]
    [SwaggerOperation(
        Summary = "Upload file",
        Description = "Uploads a file."
    )]
    public async Task<IActionResult> UploadFile(
        Guid assignmentId,
        [FromForm] UploadFileDto file)
    {
        var uploadedFile = await _assignmentService
            .UploadFileToAssignmentByTeacherId(
                CurrentUserId,
                assignmentId,
                file.File);

        return CreatedAtAction(
            nameof(GetFiles),
            new { assignmentId },
            uploadedFile);
    }

    [HttpDelete("{assignmentId:guid}/files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Delete file",
        Description = "Deletes one file by id."
    )]
    public async Task<IActionResult> DeleteFile(
        Guid assignmentId,
        Guid fileId)
    {
        await _assignmentService
            .DeleteFileFromAssignmentByTeacherId(
                CurrentUserId,
                assignmentId,
                fileId);

        return NoContent();
    }

    [HttpPost("{assignmentId:guid}/attachments/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Attach file",
        Description = "Attaches a file."
    )]
    public async Task<IActionResult> AttachFile(
        Guid assignmentId,
        Guid fileId)
    {
        await _assignmentService.AttachFileToAssignmentByTeacherId(
            CurrentUserId,
            assignmentId,
            fileId);

        return NoContent();
    }

    [HttpDelete("{assignmentId:guid}/attachments/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Detach file",
        Description = "Detaches one file."
    )]
    public async Task<IActionResult> DetachFile(
        Guid assignmentId,
        Guid fileId)
    {
        await _assignmentService.DetachFileFromAssignmentByTeacherId(
            CurrentUserId,
            assignmentId,
            fileId);

        return NoContent();
    }
}