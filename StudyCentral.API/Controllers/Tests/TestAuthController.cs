using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Constants.Tests;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Tests;

[ApiController]
[Tags("Test - Auth")]
[Route("api/public/test/auth")]
public class TestAuthController : BaseTestController
{
    private readonly IAuthService _authService;

    public TestAuthController(StudyDbContext dbContext, IAuthService authService) : base(dbContext)
    {
        _authService = authService;
    }

    [HttpGet("login-admin")]
    [SwaggerOperation(
        Summary = "Login admin",
        Description = "Logs in as the test admin user."
    )]
    public async Task<ActionResult<UserDto>> LoginAdmin()
    {
        var result = await _authService.Login(TestLoginDto.Admin);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    [HttpGet("login-teacher")]
    [SwaggerOperation(
        Summary = "Login teacher",
        Description = "Logs in as the test teacher user."
    )]
    public async Task<ActionResult<UserDto>> LoginTeacher()
    {
        var result = await _authService.Login(TestLoginDto.Teacher);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    [HttpGet("login-student")]
    [SwaggerOperation(
        Summary = "Login student",
        Description = "Logs in as the test student user."
    )]
    public async Task<ActionResult<UserDto>> LoginStudent()
    {
        var result = await _authService.Login(TestLoginDto.Student);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    // HELPER METHODS
    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
    }
}
