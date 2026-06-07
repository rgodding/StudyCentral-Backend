using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Account;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseController
{
    private readonly IUserService _userService;
    
    public AccountController(IMapper mapper, IUserService userService)
        : base(mapper)
    {
        _userService = userService;
    }
    
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var response = await _userService.GetMe(CurrentUser.Id);
        return Ok(response);
    }
    
    [HttpPut("me")]
    public async Task<ActionResult<UserDto>> UpdateMe([FromBody] UpdateUserDto dto)
    {
        var response = await _userService.UpdateMe(CurrentUser.Id, dto);
        return Ok(response);
    }
    
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordDto dto)
    {
        await _userService.ChangePassword(
            CurrentUser.Id,
            dto);

        return NoContent();
    }
    
    [HttpPost("profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        await _userService.AddProfilePicture(CurrentUser.Id, file);
        return NoContent();
    }

    [HttpDelete("profile-picture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        await _userService.DeleteProfilePicture(CurrentUser.Id);
        return NoContent();
    }
}