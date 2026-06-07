using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.ApiModels.Submission;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/submissions")]
public class TeacherSubmissionController : BaseTeacherController
{
    private readonly ISubmissionService _submissionService;
    public TeacherSubmissionController(IMapper mapper, ISubmissionService submissionService) : base(mapper)
    {
        _submissionService = submissionService;
    }
    
    [HttpGet("assignments/{assignmentId}")]
    public async Task<ActionResult<List<SubmissionDto>>> GetSubmissionsByAssignmentId(Guid assignmentId)
    {
        var submissions = await _submissionService.GetSubmissionsByAssignmentIdAndTeacherId(CurrentUser.Id, assignmentId);
        return Ok(submissions);
    }
    
    [HttpGet("{submissionId}")]
    public async Task<ActionResult<SubmissionDto>> GetSubmission(Guid submissionId)
    {
        var submission = await _submissionService.GetSubmissionByTeacherId(CurrentUser.Id, submissionId);
        return Ok(submission);
    }
    
    [HttpPost("{submissionId}/grade")]
    public async Task<IActionResult> GradeSubmission(Guid submissionId, [FromBody] GradeSubmissionRequest request)
    {
        var grade = await _submissionService.GradeSubmissionByTeacherId(CurrentUser.Id, submissionId, request);
        return Ok(grade);
    }
}