using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.StudentController;

[ApiController]
[Route("api/[controller]")]
public class StudentController : BaseStudentController
{
    private readonly IStudentService _studentService;

    public StudentController(IMapper mapper, JwtHelper jwtHelper, UserPrincipal? userPrincipal, IStudentService studentService) : base(mapper, jwtHelper, userPrincipal)
    {
        _studentService = studentService;
    }
    
    [HttpGet]
    [Route("get-courses")]
    public async Task<IActionResult> GetCoursesByUserId()
    {
        var result = await _studentService.GetCoursesByStudentId(UserPrincipal.Id);
        return Ok(result);
    }

    [HttpPost]
    [Route("upload-profile-picture")]
    public async Task<ActionResult> UploadProfilePicture(IFormFile file, [FromBody] string altText)
    {
        await _studentService.UploadProfilePicture(UserPrincipal.Id, file, altText);
        return Ok();
    }
    
}