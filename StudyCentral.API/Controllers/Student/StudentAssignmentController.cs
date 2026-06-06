using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Route("api/student/assignments")]
public class StudentAssignmentController : BaseStudentController
{
    private readonly IAssignmentService _assignmentService;

    public StudentAssignmentController(
        IMapper mapper,
        IAssignmentService assignmentService) : base(mapper)
    {
        _assignmentService = assignmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _assignmentService
            .GetAssignmentsByStudentId(CurrentUser.Id);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}")]
    public async Task<IActionResult> GetAssignment(
        Guid assignmentId)
    {
        var assignment = await _assignmentService
            .GetAssignmentByStudentId(
                CurrentUser.Id,
                assignmentId);

        return Ok(assignment);
    }

    [HttpGet("course/{courseId:guid}")]
    public async Task<IActionResult> GetAssignmentsByCourse(
        Guid courseId)
    {
        var assignments = await _assignmentService
            .GetAssignmentsByCourseIdAndStudentId(
                CurrentUser.Id,
                courseId);

        return Ok(assignments);
    }

    [HttpGet("{assignmentId:guid}/files")]
    public async Task<IActionResult> GetFiles(
        Guid assignmentId)
    {
        var files = await _assignmentService
            .GetFilesByAssignmentIdAndStudentId(
                CurrentUser.Id,
                assignmentId);

        return Ok(files);
    }
}