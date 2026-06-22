using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Tags("Student - Submissions")]
[Route("api/student/submissions")]
public class StudentSubmissionController : BaseStudentController
{
    private readonly ISubmissionService _submissionService;

    public StudentSubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get submissions",
        Description = "Gets all submissions."
    )]
    public async Task<ActionResult<List<SubmissionDto>>> GetSubmissions()
    {
        var submissions = await _submissionService.GetSubmissionsByStudentId(CurrentUserId);
        return Ok(submissions);
    }

    [HttpGet("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Get submission",
        Description = "Gets one submission by id."
    )]
    public async Task<ActionResult<SubmissionDto>> GetSubmission(Guid submissionId)
    {
        var submission = await _submissionService.GetSubmissionByStudentId(CurrentUserId, submissionId);
        return Ok(submission);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create submission",
        Description = "Creates a new submission."
    )]
    public async Task<ActionResult<SubmissionDto>> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        var submission = await _submissionService.CreateSubmissionByStudentId(CurrentUserId, dto);
        return CreatedAtAction(nameof(GetSubmission), new { submissionId = submission.Id }, submission);
    }

    [HttpPut("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Update submission",
        Description = "Updates one submission by id."
    )]
    public async Task<ActionResult<SubmissionDto>> UpdateSubmission(Guid submissionId,
        [FromBody] UpdateSubmissionDto dto)
    {
        var submission = await _submissionService.UpdateSubmissionByStudentId(CurrentUserId, submissionId, dto);
        return Ok(submission);
    }

    [HttpDelete("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Delete submission",
        Description = "Deletes one submission by id."
    )]
    public async Task<ActionResult> DeleteSubmission(Guid submissionId)
    {
        await _submissionService.DeleteSubmissionByStudentId(CurrentUserId, submissionId);
        return NoContent();
    }

    [HttpGet("{submissionId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid submissionId)
    {
        var files = await _submissionService
            .GetSubmissionFilesByStudentId(
                CurrentUserId,
                submissionId);

        return Ok(files);
    }

    [HttpPost("{submissionId:guid}/attachments/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Attach file",
        Description = "Attaches a file."
    )]
    public async Task<IActionResult> AttachFile(
        Guid submissionId,
        Guid fileId)
    {
        await _submissionService
            .AttachFileToSubmissionByStudentId(
                CurrentUserId,
                submissionId,
                fileId);

        return NoContent();
    }

    [HttpDelete("{submissionId:guid}/attachments/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Detach file",
        Description = "Detaches one file."
    )]
    public async Task<IActionResult> DetachFile(
        Guid submissionId,
        Guid fileId)
    {
        await _submissionService
            .DetachFileFromSubmissionByStudentId(
                CurrentUserId,
                submissionId,
                fileId);

        return NoContent();
    }

    [HttpPost("{submissionId:guid}/files")]
    [SwaggerOperation(
        Summary = "Upload file",
        Description = "Uploads a file."
    )]
    public async Task<ActionResult<StudyFileDto>> UploadFile(
        Guid submissionId,
        [FromForm] UploadFileDto dto)
    {
        var uploadedFile = await _submissionService
            .UploadSubmissionFileByStudentId(
                CurrentUserId,
                submissionId,
                dto.File);

        return CreatedAtAction(
            nameof(GetFiles),
            new { submissionId },
            uploadedFile);
    }

    [HttpDelete("{submissionId:guid}/files/{fileId:guid}")]
    [SwaggerOperation(
        Summary = "Delete file",
        Description = "Deletes one file by id."
    )]
    public async Task<IActionResult> DeleteFile(
        Guid submissionId,
        Guid fileId)
    {
        await _submissionService
            .DeleteSubmissionFileByStudentId(
                CurrentUserId,
                submissionId,
                fileId);

        return NoContent();
    }
}