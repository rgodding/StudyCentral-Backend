using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Course;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Route("api/admin/courses")]
public class AdminCourseController : BaseAdminController
{
    private readonly ICourseService _courseService;

    public AdminCourseController(
        IMapper mapper,
        ICourseService courseService)
        : base(mapper)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetCourses()
    {
        var courses = await _courseService.GetAll();

        return Ok(courses);
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<CourseDto>> GetCourse(
        Guid courseId)
    {
        var course = await _courseService.GetById(
            courseId);

        return Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> CreateCourse(
        [FromBody] CreateCourseDto dto)
    {
        var course = await _courseService.Create(
            dto);

        return CreatedAtAction(
            nameof(GetCourse),
            new { courseId = course.Id },
            course);
    }

    [HttpPut("{courseId}")]
    public async Task<ActionResult<CourseDto>> UpdateCourse(
        Guid courseId,
        [FromBody] AdminUpdateCourseDto dto)
    {
        var course = await _courseService.AdminUpdateCourse(
            courseId,
            dto);

        return Ok(course);
    }

    [HttpDelete("{courseId}")]
    public async Task<IActionResult> DeleteCourse(
        Guid courseId)
    {
        await _courseService.Delete(
            courseId);

        return NoContent();
    }
}