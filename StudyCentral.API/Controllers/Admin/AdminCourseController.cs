using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.Course;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Tags("Admin - Courses")]
[Route("api/admin/courses")]
public class AdminCourseController : BaseAdminController
{
    private readonly ICourseService _courseService;

    public AdminCourseController(ICourseService courseService)
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
        var courses = await _courseService.GetAll();

        return Ok(courses);
    }

    [HttpGet("{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Get course",
        Description = "Gets one course by id."
    )]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
    {
        var course = await _courseService.GetById(courseId);
        return Ok(course);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create course",
        Description = "Creates a new course."
    )]
    public async Task<ActionResult<CourseDto>> CreateCourse([FromBody] CreateCourseDto dto)
    {
        var course = await _courseService.Create(dto);

        return CreatedAtAction(
            nameof(GetCourse),
            new { courseId = course.Id },
            course);
    }

    [HttpPut("{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Update course",
        Description = "Updates one course by id."
    )]
    public async Task<ActionResult<CourseDto>> UpdateCourse(Guid courseId, [FromBody] AdminUpdateCourseDto dto)
    {
        var course = await _courseService.AdminUpdateCourse(courseId, dto);
        return Ok(course);
    }

    [HttpDelete("{courseId:guid}")]
    [SwaggerOperation(
        Summary = "Delete course",
        Description = "Deletes one course by id."
    )]
    public async Task<IActionResult> DeleteCourse(Guid courseId)
    {
        await _courseService.Delete(courseId);
        return NoContent();
    }
}
