using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.DTOs.Auth;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Public;

[ApiController]
[Tags("Auth")]
[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register",
        Description = "Creates a new account."
    )]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.Register(dto);

        SetAuthCookie(result.Token);

        return Ok(result.User);
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login",
        Description = "Signs in a user."
    )]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.Login(dto);

        SetAuthCookie(result.Token);

        return Ok(result.User);
    }

    [HttpPost("logout")]
    [SwaggerOperation(
        Summary = "Logout",
        Description = "Signs out the current user."
    )]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            "access_token",
            new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None
            });

        return NoContent();
    }

    [HttpGet("generator-hash")]
    [SwaggerOperation(
        Summary = "Generate hash",
        Description = "Generates a password hash."
    )]
    public ActionResult<string> GenerateHash(string value)
    {
        var hash = PasswordHelper.HashPassword(value);
        return Ok(hash);
    }

    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}