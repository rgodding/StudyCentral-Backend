using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Submission;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Route("api/admin/submissions")]
public class AdminSubmissionController : BaseAdminController
{
    private readonly ISubmissionService _submissionService;

    public AdminSubmissionController(
        IMapper mapper,
        ISubmissionService submissionService)
        : base(mapper)
    {
        _submissionService = submissionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubmissionDto>>> GetSubmissions()
    {
        var submissions = await _submissionService.GetAll();

        return Ok(submissions);
    }

    [HttpGet("{submissionId}")]
    public async Task<ActionResult<SubmissionDto>> GetSubmission(
        Guid submissionId)
    {
        var submission = await _submissionService.GetById(
            submissionId);

        return Ok(submission);
    }

    [HttpPut("{submissionId}")]
    public async Task<ActionResult<SubmissionDto>> UpdateSubmission(
        Guid submissionId,
        [FromBody] AdminUpdateSubmissionDto dto)
    {
        var submission = await _submissionService.AdminUpdateSubmission(
            submissionId,
            dto);

        return Ok(submission);
    }

    [HttpDelete("{submissionId}")]
    public async Task<IActionResult> DeleteSubmission(
        Guid submissionId)
    {
        await _submissionService.Delete(
            submissionId);

        return NoContent();
    }
}