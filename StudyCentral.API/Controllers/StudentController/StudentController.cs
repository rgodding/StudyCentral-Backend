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

    [HttpPost]
    [Route("upload-profile-picture")]
    public async Task<ActionResult> UploadProfilePicture(IFormFile file, [FromBody] string altText)
    {
        await _studentService.UploadProfilePicture(UserPrincipal.Id, file, altText);
        return Ok();
    }
    
}