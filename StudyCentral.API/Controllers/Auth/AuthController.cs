using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.Dtos.Auth;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
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
}