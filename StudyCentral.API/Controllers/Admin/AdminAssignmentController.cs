using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Assignment;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Route("api/admin/assignments")]
public class AdminAssignmentController : BaseAdminController
{
    private readonly IAssignmentService _assignmentService;

    public AdminAssignmentController(
        IMapper mapper,
        IAssignmentService assignmentService)
        : base(mapper)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AssignmentDto>>> GetAssignments()
    {
        var assignments = await _assignmentService.GetAll();

        return Ok(assignments);
    }

    [HttpGet("{assignmentId}")]
    public async Task<ActionResult<AssignmentDto>> GetAssignment(
        Guid assignmentId)
    {
        var assignment = await _assignmentService.GetById(
            assignmentId);

        return Ok(assignment);
    }

    [HttpPut("{assignmentId}")]
    public async Task<ActionResult<AssignmentDto>> UpdateAssignment(
        Guid assignmentId,
        [FromBody] AdminUpdateAssignmentDto dto)
    {
        var assignment = await _assignmentService.AdminUpdateAssignment(
            assignmentId,
            dto);

        return Ok(assignment);
    }

    [HttpDelete("{assignmentId}")]
    public async Task<IActionResult> DeleteAssignment(
        Guid assignmentId)
    {
        await _assignmentService.Delete(
            assignmentId);

        return NoContent();
    }

    [HttpGet("{assignmentId}/files")]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(
        Guid assignmentId)
    {
        var files = await _assignmentService.GetFiles(
            assignmentId);

        return Ok(files);
    }
}