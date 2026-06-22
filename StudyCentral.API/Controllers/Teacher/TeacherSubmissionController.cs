using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Tags("Teacher - Submissions")]
[Route("api/teacher/submissions")]
public class TeacherSubmissionController : BaseTeacherController
{
    private readonly ISubmissionService _submissionService;

    public TeacherSubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("assignments/{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Get submissions by assignment id",
        Description = "Gets submissions for one assignment."
    )]
    public async Task<ActionResult<List<SubmissionDto>>> GetSubmissionsByAssignmentId(Guid assignmentId)
    {
        var submissions =
            await _submissionService.GetSubmissionsByAssignmentIdAndTeacherId(CurrentUserId, assignmentId);
        return Ok(submissions);
    }

    [HttpGet("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Get submission",
        Description = "Gets one submission by id."
    )]
    public async Task<ActionResult<SubmissionDto>> GetSubmission(Guid submissionId)
    {
        var submission = await _submissionService.GetSubmissionByTeacherId(CurrentUserId, submissionId);
        return Ok(submission);
    }

    [HttpPost("{submissionId:guid}/grade")]
    [SwaggerOperation(
        Summary = "Grade submission",
        Description = "Grades one submission."
    )]
    public async Task<IActionResult> GradeSubmission(Guid submissionId, [FromBody] GradeSubmissionDto dto)
    {
        var grade = await _submissionService.GradeSubmissionByTeacherId(CurrentUserId, submissionId, dto);
        return Ok(grade);
    }

    [HttpGet("{submissionId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(Guid submissionId)
    {
        var files = await _submissionService.GetSubmissionFilesByTeacherId(CurrentUserId, submissionId);
        return Ok(files);
    }
}