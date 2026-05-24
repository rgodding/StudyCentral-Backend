using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.Dtos.Auth;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "IsUser")]
public class AuthController : BaseUserController
{
    private readonly IAuthService _authService;
    
    public AuthController(IMapper mapper, IAuthService authService) : base(mapper)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [Route("sign-in")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> SignIn([FromBody] SignInDto signInDto)
    {
        var response = await _authService.SignIn(signInDto);
        return Ok(response);
        
    }
    
    [HttpPost]
    [Route("sign-up")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> SignUp([FromBody] SignUpDto signUpDto)
    {
        var response = await _authService.SignUp(signUpDto);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("me")]
    public async Task<ActionResult<AuthResponseDto>> GetMe()
    {
        var response = await _authService.GetCurrentUser(UserPrincipal.Id);
        return Ok(response);
    }
    
}