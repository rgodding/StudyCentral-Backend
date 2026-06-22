using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Tags("Student - Assignments")]
[Route("api/student/assignments")]
public class StudentAssignmentController : BaseStudentController
{
    private readonly IAssignmentService _assignmentService;

    public StudentAssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get assignments",
        Description = "Gets all assignments."
    )]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _assignmentService
            .GetAssignmentsByStudentId(CurrentUserId);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}")]
    [SwaggerOperation(
        Summary = "Get assignment",
        Description = "Gets one assignment by id."
    )]
    public async Task<IActionResult> GetAssignment(
        Guid assignmentId)
    {
        var assignment = await _assignmentService
            .GetAssignmentByIdAndStudentId(
                CurrentUserId,
                assignmentId);

        return Ok(assignment);
    }

    [HttpGet("course/{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get assignments by course",
        Description = "Gets assignments for one course."
    )]
    public async Task<IActionResult> GetAssignmentsByCourse(
        Guid courseId)
    {
        var assignments = await _assignmentService
            .GetAssignmentsByCourseIdAndStudentId(
                CurrentUserId,
                courseId);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}/files")]
    [SwaggerOperation(
        Summary = "Get files",
        Description = "Gets linked files."
    )]
    public async Task<IActionResult> GetFiles(
        Guid assignmentId)
    {
        var files = await _assignmentService
            .GetFilesByAssignmentIdAndStudentId(
                CurrentUserId,
                assignmentId);

        return Ok(files);
    }
}