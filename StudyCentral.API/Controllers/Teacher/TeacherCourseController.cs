using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/courses")]
public class TeacherCourseController : BaseTeacherController
{
    private readonly ICourseService _courseService;
    public TeacherCourseController(IMapper mapper, ICourseService courseService)
        : base(mapper)
    {
        _courseService = courseService;
    }

    // ------------------
    // Course Management
    // ------------------

    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetMyCourses()
    { 
        var courses = await _courseService.GetCoursesByTeacherId(CurrentUser.Id);
        return Ok(courses);
    }

    [HttpGet("{courseId:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
    {
        var course = await _courseService.GetCourseByTeacherId(CurrentUser.Id, courseId);
        return Ok(course);
    }

    [HttpPut("{courseId:guid}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(
        Guid courseId,
        [FromBody] UpdateCourseDto dto)
    {
        var course = await _courseService.UpdateCourseByTeacherId(CurrentUser.Id, courseId, dto);
        return Ok(course);
    }

    // ------------------
    // Student Management
    // ------------------

    [HttpGet("{courseId:guid}/students")]
    public async Task<ActionResult<List<UserDto>>> GetStudents(Guid courseId)
    {
        var students = await _courseService.GetStudentsByTeacherId(CurrentUser.Id, courseId);
        return Ok(students);
    }

    [HttpPost("{courseId:guid}/students/{studentId:guid}")]
    public async Task<IActionResult> AddStudent(
        Guid courseId,
        Guid studentId)
    {
        await _courseService.AddStudentByTeacherId(CurrentUser.Id, courseId, studentId);
        return NoContent();
    }

    [HttpDelete("{courseId:guid}/students/{studentId:guid}")]
    public async Task<IActionResult> RemoveStudent(
        Guid courseId,
        Guid studentId)
    {
        await _courseService.RemoveStudentByTeacherId(CurrentUser.Id, courseId, studentId);
        return NoContent();
    }
}