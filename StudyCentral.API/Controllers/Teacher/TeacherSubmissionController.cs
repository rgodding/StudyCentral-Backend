using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    
    [HttpGet("assignment/{assignmentId}")]
    public async Task<IActionResult> GetSubmissionsByAssignmentId(Guid assignmentId)
    {
        var submissions = await _submissionService.GetSubmissionsByAssignmentIdAndTeacherId(CurrentUser.Id, assignmentId);
        return Ok(submissions);
    }
    
    [HttpGet("{submissionId}")]
    public async Task<IActionResult> GetSubmission(Guid submissionId)
    {
        var submission = await _submissionService.GetSubmissionByTeacherId(CurrentUser.Id, submissionId);
        return Ok(submission);
    }
    
    [HttpGet("{submissionId}/grade")]
    public async Task<IActionResult> GetSubmissionGrade(Guid submissionId)
    {
        var grade = await _submissionService.GradeSubmissionByTeacherId(CurrentUser.Id, submissionId, null);
        return Ok(grade);
    }
}