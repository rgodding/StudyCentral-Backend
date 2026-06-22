using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Assignment;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Models.DTOs.StudyFile;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Tags("Admin - Assignments")]
[Route("api/admin/assignments")]
public class AdminAssignmentController : BaseAdminController
{
    private readonly IAssignmentService _assignmentService;

    public AdminAssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get assignments",
        Description = "Gets all assignments."
    )]
    public async Task<ActionResult<List<AssignmentDto>>> GetAssignments()
    {
        var assignments = await _assignmentService.GetAll();
        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Get assignment",
        Description = "Gets one assignment by id."
    )]
    public async Task<ActionResult<AssignmentDto>> GetAssignment(Guid assignmentId)
    {
        var assignment = await _assignmentService.GetById(assignmentId);
        return Ok(assignment);
    }

    [HttpPut("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Update assignment",
        Description = "Updates one assignment by id."
    )]
    public async Task<ActionResult<AssignmentDto>> UpdateAssignment(Guid assignmentId, [FromBody] AdminUpdateAssignmentDto dto)
    {
        var assignment = await _assignmentService.AdminUpdateAssignment(assignmentId, dto);
        return Ok(assignment);
    }

    [HttpDelete("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Delete assignment",
        Description = "Deletes one assignment by id."
    )]
    public async Task<IActionResult> DeleteAssignment(Guid assignmentId)
    {
        await _assignmentService.Delete(assignmentId);
        return NoContent();
    }

    [HttpGet("{assignmentId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get assignment files",
        Description = "Gets files linked to one assignment."
    )]
    public async Task<ActionResult<List<StudyFileDto>>> GetFiles(Guid assignmentId)
    {
        var files = await _assignmentService.GetFiles(assignmentId);
        return Ok(files);
    }
}