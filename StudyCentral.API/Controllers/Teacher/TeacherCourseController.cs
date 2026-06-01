using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/[controller]")]
public class TeacherCourseController : BaseTeacherController
{
    private readonly ICourseService _courseService;
    
    public TeacherCourseController(IMapper mapper, ICourseService courseService) : base(mapper)
    {
        _courseService = courseService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var result = await _courseService.GetAllCourses();
        var response = _mapper.Map<List<CourseDto>>(result);
        return Ok(response);
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourseById(Guid courseId)
    {
        var result = await _courseService.GetCourse(courseId);
        var response = _mapper.Map<CourseDto>(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("{courseId}")]
    public async Task<IActionResult> UpdateCourse(Guid courseId, [FromBody] UpdateCourseDto request)
    {
        var result = await _courseService.UpdateCourse(courseId, request);
        return Ok(result);   
    }
    
    [HttpGet]
    [Route("{courseId}/students")]
    public async Task<IActionResult> GetEnrolledStudents(Guid courseId)
    {
        var response = await _courseService.GetEnrolledStudents(courseId);
        return Ok(response);  
    }
    
    [HttpPost]
    [Route("{courseId}/students/{studentId}")]
    public async Task<IActionResult> EnrollStudent(Guid courseId, Guid studentId)
    {
        await _courseService.EnrollStudent(courseId, studentId);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{courseId}/students/{studentId}")]
    public async Task<IActionResult> UnenrollStudent(Guid courseId, Guid studentId)
    {
        await _courseService.UnenrollStudent(courseId, studentId);
        return Ok();
    }
    
}