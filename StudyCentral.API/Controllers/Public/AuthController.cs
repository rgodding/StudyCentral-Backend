using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.DTOs.Auth;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Public;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    
    public AuthController(IMapper mapper, IAuthService authService) : base(mapper)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.Register(dto);
        
        SetAuthCookie(result.Token);
        
        return Ok(result.User);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.Login(dto);

        SetAuthCookie(result.Token);
        
        return Ok(result.User);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            "access_token",
            new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

        return NoContent();
    }
    
    [HttpGet("generator-hash")]
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
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}