using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.Dtos.Courses;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Route("api/teacher/[controller]")]
public class StudentCourseController : BaseStudentController
{
    
    private readonly ICourseService _courseService;
    
    public StudentCourseController(IMapper mapper, ICourseService courseService) : base(mapper)
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
}