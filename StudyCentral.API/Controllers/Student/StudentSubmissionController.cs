using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Route("api/student/submissions")]
public class StudentSubmissionController : BaseStudentController
{
    private readonly ISubmissionService _submissionService;

    public StudentSubmissionController(IMapper mapper, ISubmissionService submissionService) : base(mapper)
    {
        _submissionService = submissionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubmissionDto>>> GetSubmissions()
    {
        var submissions = await _submissionService.GetSubmissionsByStudentId(CurrentUser.Id);
        return Ok(submissions);
    }

    [HttpGet("{submissionId}")]
    public async Task<ActionResult<SubmissionDto>> GetSubmission(Guid submissionId)
    {
        var submission = await _submissionService.GetSubmissionByStudentId(CurrentUser.Id, submissionId);
        return Ok(submission);
    }

    [HttpPost]
    public async Task<ActionResult<SubmissionDto>> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        var submission = await _submissionService.CreateSubmissionByStudentId(CurrentUser.Id, dto);
        return CreatedAtAction(nameof(GetSubmission), new { submissionId = submission.Id }, submission);
    }

    [HttpPut("{submissionId:guid}")]
    public async Task<ActionResult<SubmissionDto>> UpdateSubmission(Guid submissionId,
        [FromBody] UpdateSubmissionDto dto)
    {
        var submission = await _submissionService.UpdateSubmissionByStudentId(CurrentUser.Id, submissionId, dto);
        return Ok(submission);
    }

    [HttpDelete("{submissionId:guid}")]
    public async Task<ActionResult> DeleteSubmission(Guid submissionId)
    {
        await _submissionService.DeleteSubmissionByStudentId(CurrentUser.Id, submissionId);
        return NoContent();
    }

    [HttpGet("{submissionId:guid}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid submissionId)
    {
        var files = await _submissionService
            .GetSubmissionFilesByStudentId(
                CurrentUser.Id,
                submissionId);

        return Ok(files);
    }

    [HttpPost("{submissionId:guid}/attachments/{fileId:guid}")]
    public async Task<IActionResult> AttachFile(
        Guid submissionId,
        Guid fileId)
    {
        await _submissionService
            .AttachFileToSubmissionByStudentId(
                CurrentUser.Id,
                submissionId,
                fileId);

        return NoContent();
    }

    [HttpDelete("{submissionId:guid}/attachments/{fileId:guid}")]
    public async Task<IActionResult> DetachFile(
        Guid submissionId,
        Guid fileId)
    {
        await _submissionService
            .DetachFileFromSubmissionByStudentId(
                CurrentUser.Id,
                submissionId,
                fileId);

        return NoContent();
    }

    [HttpPost("{submissionId:guid}/files")]
    public async Task<ActionResult<StudyFileDto>> UploadFile(
        Guid submissionId,
        [FromForm] UploadFileDto dto)
    {
        var uploadedFile = await _submissionService
            .UploadSubmissionFileByStudentId(
                CurrentUser.Id,
                submissionId,
                dto.File);

        return CreatedAtAction(
            nameof(GetFiles),
            new { submissionId },
            uploadedFile);
    }

    [HttpDelete("{submissionId}/files/{fileId}")]
    public async Task<IActionResult> DeleteFile(
        Guid submissionId,
        Guid fileId)
    {
        await _submissionService
            .DeleteSubmissionFileByStudentId(
                CurrentUser.Id,
                submissionId,
                fileId);

        return NoContent();
    }
}