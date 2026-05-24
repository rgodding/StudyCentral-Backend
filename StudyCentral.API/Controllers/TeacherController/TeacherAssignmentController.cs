using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/teacher/assignments")]
public class TeacherAssignmentController : BaseTeacherController
{
    public TeacherAssignmentController(IMapper mapper, ITeacherService teacherService) : base(mapper, teacherService)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments()
    {
        var assignments = await _teacherService.GetAssignmentsByTeacherId(UserPrincipal.Id);
        return Ok(assignments);
        
    }

    [HttpGet]
    [Route("{assignmentId}")]
    public IActionResult GetAssignmentById(int assignmentId)
    {
        return Ok($"Assignment with ID: {assignmentId}");
    }

    [HttpPatch]
    [Route("{assignmentId}")]
    public IActionResult UpdateAssignment(int assignmentId)
    {
        return Ok("Assignment updated");
    }

    [HttpDelete]
    [Route("{assignmentId}")]
    public IActionResult DeleteAssignment(int assignmentId)
    {
        return Ok("Assignment deleted");
    }
    
    [HttpGet]
    [Route("{assignmentId}/submissions")]
    public IActionResult GetAssignmentSubmissions(int assignmentId)
    {
        return Ok($"Submissions for Assignment with ID: {assignmentId}");
    }
    
    [HttpPost]
    [Route("{assignmentId}/submissions/{submissionId}/grade")]
    public IActionResult GradeSubmission(int assignmentId, int submissionId)
    {
        return Ok("Submission graded");
    }
}