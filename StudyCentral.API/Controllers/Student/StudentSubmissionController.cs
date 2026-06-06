using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<SubmissionDto>> GetSubmissions()
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
        return Ok(submission);
    }

    [HttpPut("{submissionId}")]
    public async Task<ActionResult<SubmissionDto>> UpdateSubmission(Guid submissionId,
        [FromBody] UpdateSubmissionDto dto)
    {
        var submission = await _submissionService.UpdateSubmissionByStudentId(CurrentUser.Id, submissionId, dto);
        return Ok(submission);
    }
    
    [HttpDelete("{submissionId}")]
    public async Task<ActionResult> DeleteSubmission(Guid submissionId)
    {
        await _submissionService.DeleteSubmissionByStudentId(CurrentUser.Id, submissionId);
        return NoContent();
    }
}