using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.ApiModels.AuthModels;
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
    [Route("get-user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var user = await _userService.GetUserInfo(UserPrincipal.Id);
        var response = _mapper.Map<GetUserInfoResponseModel>(user);
        return Ok(response);
    }

    [HttpGet]
    [Route("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestModel request)
    {
        var user = await _userService.GetUserByEmail(request.Email);
        
        // Check if the password is correct
        if (!PasswordHelper.VerifyHash(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid password");
        }
        
        var token = _jwtHelper.GenerateToken(user);

        var response = new SignInResponseModel
        {
            Token = token,
        };
        
        return Ok(response);
    }

    [HttpPost]
    [Route("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel request)
    {
        var user = await _userService.CreateUser(request);
        return Ok(user);
    }
}