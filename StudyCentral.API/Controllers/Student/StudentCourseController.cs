using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Course;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Student;

[ApiController]
[Route("api/student/[controller]")]
public class StudentCourseController : BaseStudentController
{
    private readonly ICourseService _courseService;
    
    public StudentCourseController(IMapper mapper, ICourseService courseService) : base(mapper)
    {
        _courseService = courseService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetCourses()
    {
        var courses = await _courseService.GetCoursesByStudentId(CurrentUser.Id);
        return Ok(courses);
    }
    
    [HttpGet("{courseId:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourse(Guid courseId)
    {
        var course = await _courseService.GetCourseByStudentId(CurrentUser.Id, courseId);
        return Ok(course);
    }
    
    [HttpGet("{courseId:guid}/students")]
    public async Task<ActionResult<List<UserDto>>> GetStudentsByCourseId(Guid courseId)
    {
        var students = await _courseService.GetStudentsByCourseId(CurrentUser.Id, courseId);
        return Ok(students);
    }
}