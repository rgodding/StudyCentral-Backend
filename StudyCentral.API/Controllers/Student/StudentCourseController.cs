using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Tags("Student - Courses")]
[Route("api/student/courses")]
public class StudentCourseController : BaseStudentController
{
    private readonly ICourseService _courseService;

    public StudentCourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get courses",
        Description = "Gets all courses."
    )]
    public async Task<ActionResult<List<CourseDto>>> GetCourses()
    {
        var courses = await _courseService.GetCoursesByStudentId(CurrentUserId);
        return Ok(courses);
    }

    [HttpGet("{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get course",
        Description = "Gets one course by id."
    )]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
    {
        var course = await _courseService.GetCourseByStudentId(CurrentUserId, courseId);
        return Ok(course);
    }

    [HttpGet("{courseId:guid}/students")]
    [SwaggerOperation(
        Summary = "Get students by course id",
        Description = "Gets students in one course."
    )]
    public async Task<ActionResult<List<UserDto>>> GetStudentsByCourseId(Guid courseId)
    {
        var students = await _courseService.GetStudentsByCourseId(CurrentUserId, courseId);
        return Ok(students);
    }
}