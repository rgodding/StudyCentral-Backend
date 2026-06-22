using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Tags("Teacher - Courses")]
[Route("api/teacher/courses")]
public class TeacherCourseController : BaseTeacherController
{
    private readonly ICourseService _courseService;
    public TeacherCourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // ------------------
    // Course Management
    // ------------------

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get my courses",
        Description = "Gets the signed-in teacher’s courses."
    )]
    public async Task<ActionResult<List<CourseDto>>> GetMyCourses()
    { 
        var courses = await _courseService.GetCoursesByTeacherId(CurrentUserId);
        return Ok(courses);
    }

    [HttpGet("{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get course",
        Description = "Gets one course by id."
    )]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
    {
        var course = await _courseService.GetCourseByTeacherId(CurrentUserId, courseId);
        return Ok(course);
    }

    [HttpPut("{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Update course",
        Description = "Updates one course by id."
    )]
    public async Task<ActionResult<CourseDto>> UpdateCourse(
        Guid courseId,
        [FromBody] UpdateCourseDto dto)
    {
        var course = await _courseService.UpdateCourseByTeacherId(CurrentUserId, courseId, dto);
        return Ok(course);
    }

    // ------------------
    // Student Management
    // ------------------

    [HttpGet("{courseId:guid}/students")]
    [SwaggerOperation(
        Summary = "Get students",
        Description = "Gets students in one course."
    )]
    public async Task<ActionResult<List<UserDto>>> GetStudents(Guid courseId)
    {
        var students = await _courseService.GetStudentsByTeacherId(CurrentUserId, courseId);
        return Ok(students);
    }

    [HttpPost("{courseId:guid}/students/{studentId:guid}")]
    [SwaggerOperation(
        Summary = "Add student",
        Description = "Adds a student to one course."
    )]
    public async Task<IActionResult> AddStudent(
        Guid courseId,
        Guid studentId)
    {
        await _courseService.AddStudentByTeacherId(CurrentUserId, courseId, studentId);
        return NoContent();
    }

    [HttpDelete("{courseId:guid}/students/{studentId:guid}")]
    [SwaggerOperation(
        Summary = "Remove student",
        Description = "Removes a student from one course."
    )]
    public async Task<IActionResult> RemoveStudent(
        Guid courseId,
        Guid studentId)
    {
        await _courseService.RemoveStudentByTeacherId(CurrentUserId, courseId, studentId);
        return NoContent();
    }
}