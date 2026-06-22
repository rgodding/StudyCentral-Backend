using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Submission;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Tags("Admin - Submissions")]
[Route("api/admin/submissions")]
public class AdminSubmissionController : BaseAdminController
{
    private readonly ISubmissionService _submissionService;

    public AdminSubmissionController(ISubmissionService submissionService)
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
        var submissions = await _submissionService.GetAll();
        return Ok(submissions);
    }

    [HttpGet("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Get submission",
        Description = "Gets one submission by id."
    )]
    public async Task<ActionResult<SubmissionDto>> GetSubmission(Guid submissionId)
    {
        var submission = await _submissionService.GetById(submissionId);
        return Ok(submission);
    }

    [HttpPut("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Update submission",
        Description = "Updates one submission by id."
    )]
    public async Task<ActionResult<SubmissionDto>> UpdateSubmission(Guid submissionId, [FromBody] AdminUpdateSubmissionDto dto)
    {
        var submission = await _submissionService.AdminUpdateSubmission(submissionId, dto);
        return Ok(submission);
    }

    [HttpDelete("{submissionId:guid}")]
    [SwaggerOperation(
        Summary = "Delete submission",
        Description = "Deletes one submission by id."
    )]
    public async Task<IActionResult> DeleteSubmission(Guid submissionId)
    {
        await _submissionService.Delete(submissionId);
        return NoContent();
    }
}