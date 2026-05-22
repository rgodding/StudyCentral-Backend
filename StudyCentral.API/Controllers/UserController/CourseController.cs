using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Controllers.StudentController;
using StudyCentral.API.Models.ApiModels.CourseModels;
using StudyCentral.API.Models.DtoModels;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.UserController;

[ApiController]
[Route("api/[controller]")]
public class CourseController : BaseUserController
{
    private readonly ICourseService _courseService;


    public CourseController(IMapper mapper, JwtHelper jwtHelper, ICourseService courseService) : base(mapper, jwtHelper)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var result = await _courseService.GetCourses();
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourseById(Guid courseId)
    {
        var result = await _courseService.GetCourseById(courseId);
        return Ok(result);
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequestModel request)
    {
        Console.WriteLine("Creating course");
        var result = await _courseService.CreateCourse(request);
        var response = _mapper.Map<CourseDto>(result);
        return Ok(response);
    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateCourse(Guid courseId, [FromBody] UpdateCourseRequestModel request)
    {
        var result = await _courseService.UpdateCourse(courseId, request);
        var response = _mapper.Map<CourseDto>(result);
        return Ok(response);
    }
    
    [HttpDelete]
    [Route("{courseId}")]
    public async Task<IActionResult> DeleteCourse(Guid courseId)
    {
        await _courseService.DeleteCourse(courseId);
        return Ok();
    }
    
    [HttpPut]
    [Route("add-student/{courseId}/{studentId}")]
    public async Task<IActionResult> AddStudentToCourse(Guid courseId, Guid studentId)
    {
        await _courseService.AddStudentToCourse(courseId, studentId);
        return Ok();
    }
    
}