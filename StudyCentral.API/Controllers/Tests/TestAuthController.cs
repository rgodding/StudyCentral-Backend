using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Constants.Tests;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Tests;

[Tags("Test - Auth")]
[Route("api/test/auth")]
public class TestAuthController : BaseTestController
{
    private readonly IAuthService _authService;
    
    public TestAuthController(IMapper mapper, StudyDbContext dbContext, IAuthService authService) : base(mapper, dbContext)
    {
        _authService = authService;
    }

    [HttpGet("login-admin")]
    public async Task<ActionResult<UserDto>> LoginAdmin()
    {
        var result = await _authService.Login(TestLoginDto.Admin);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    [HttpGet("login-teacher")]
    public async Task<ActionResult<UserDto>> LoginTeacher()
    {
        var result = await _authService.Login(TestLoginDto.Teacher);
        SetAuthCookie(result.Token);
        return Ok(result.User);
    }

    [HttpGet("login-student")]
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