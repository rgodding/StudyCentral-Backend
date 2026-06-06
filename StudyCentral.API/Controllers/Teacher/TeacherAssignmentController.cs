using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Assignment;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/assignment")]
public class TeacherAssignmentController : BaseTeacherController
{
    private readonly IAssignmentService _assignmentService;
    public TeacherAssignmentController(IMapper mapper, IAssignmentService assignmentService) : base(mapper)
    {
        _assignmentService = assignmentService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<AssignmentDto>>> GetAll()
    {
        var assignments = await _assignmentService.GetAssignmentsByTeacherId(CurrentUser.Id);
        return Ok(assignments);
    }
    
    [HttpGet("{assignmentId:guid}")]
    public async Task<ActionResult<AssignmentDto>> Get(Guid assignmentId)
    {
        var assignment = await _assignmentService.GetAssignmentByTeacherId(CurrentUser.Id, assignmentId);
        return Ok(assignment);
    }
    
    [HttpPost]
    public async Task<ActionResult<AssignmentDto>> Create([FromBody] CreateAssignmentDto dto)
    {
        var assignment = await _assignmentService.CreateAssignmentByTeacherId(CurrentUser.Id, dto);
        return Ok(assignment);
    }
    
    [HttpPut("{assignmentId:guid}")]
    public async Task<ActionResult<AssignmentDto>> Update(Guid assignmentId, [FromBody] UpdateAssignmentDto dto)
    {
        var assignment = await _assignmentService.UpdateAssignmentByTeacherId(CurrentUser.Id, assignmentId, dto);
        return Ok(assignment);
    }
    
    [HttpDelete("{assignmentId:guid}")]
    public async Task<ActionResult> Delete(Guid assignmentId)
    { 
        await _assignmentService.DeleteAssignmentByTeacherId(CurrentUser.Id, assignmentId);
        return NoContent();
    }
    
}