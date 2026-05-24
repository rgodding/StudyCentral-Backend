using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/teacher/courses")]
public class TeacherCourseController : BaseTeacherController
{
    public TeacherCourseController(IMapper mapper, ITeacherService teacherService) : base(mapper, teacherService)
    {
    }

    [HttpGet]
    public IActionResult GetCourses()
    {
        return Ok("Courses");
    }
    
    [HttpGet]
    [Route("{courseId}")]
    public IActionResult GetCourseById(int courseId)
    {
        return Ok($"Course with ID: {courseId}");
    }
    
    [HttpGet]
    [Route("{courseId}/students")]
    public IActionResult GetCourseStudents(int courseId)
    {
        return Ok($"Students for Course with ID: {courseId}");
    }
    
    [HttpPatch]
    [Route("{courseId}")]
    public IActionResult UpdateCourse(int courseId)
    {
        return Ok("Course updated");
    }
    
    [HttpGet]
    [Route("{courseId}/assignments")]
    public IActionResult GetCourseAssignments(int courseId)
    {
        return Ok($"Assignments for Course with ID: {courseId}");
    }
    
    [HttpPost]
    [Route("{courseId}/assignments")]
    public IActionResult AddAssignment(int courseId)
    {
        return Ok("Assignment added");
    }
    
    [HttpPost]
    [Route("{courseId}/students")]
    public IActionResult AddStudent(int courseId)
    {
        return Ok("Student added");
    }
    
}