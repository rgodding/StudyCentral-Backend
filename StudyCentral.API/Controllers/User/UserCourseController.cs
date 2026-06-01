using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.User;

[ApiController]
[Route("api/student/[controller]")]
public class UserCourseController : BaseUserController
{
    public UserCourseController(IMapper mapper) : base(mapper)
    {
    }
    
    [HttpGet]
    public IActionResult GetCourses()
    {
        return Ok();
    }

    [HttpGet]
    [Route("{courseId}")]
    public IActionResult GetCourseById(Guid courseId)
    {
        return Ok();
    }

    [HttpGet]
    [Route("enrolled")]
    public IActionResult GetEnrolledCourses()
    {
        return Ok();
    }
}