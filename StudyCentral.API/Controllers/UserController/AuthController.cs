using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.UserController;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseUserController
{
    
    private readonly IUserService _userService;


    public AuthController(IMapper mapper, JwtHelper jwtHelper, IUserService userService) : base(mapper, jwtHelper)
    {
        _userService = userService;
    }

    [HttpGet]
    [Route("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn()
    {
        return Ok("22");
    }

    [HttpPost]
    [Route("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp()
    {
        return Ok();
    }
}