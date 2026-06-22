using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Account;

[Authorize]
[ApiController]
[Tags("Account")]
[Route("api/account")]
public class AccountController : BaseController
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Get me",
        Description = "Gets the signed-in user's profile."
    )]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var response = await _userService.GetMe(CurrentUserId);
        return Ok(response);
    }

    [HttpPut("me")]
    [SwaggerOperation(
        Summary = "Update me",
        Description = "Updates the signed-in user's profile."
    )]
    public async Task<ActionResult<UserDto>> UpdateMe([FromBody] UpdateUserDto dto)
    {
        var response = await _userService.UpdateMe(CurrentUserId, dto);
        return Ok(response);
    }

    [HttpPut("password")]
    [SwaggerOperation(
        Summary = "Change password",
        Description = "Changes the signed-in user's password."
    )]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        await _userService.ChangePassword(CurrentUserId, dto);
        return NoContent();
    }

    [HttpPost("profile-picture")]
    [SwaggerOperation(
        Summary = "Upload profile picture",
        Description = "Uploads a profile picture for the signed-in user."
    )]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        await _userService.AddProfilePicture(CurrentUserId, file);
        return NoContent();
    }

    [HttpDelete("profile-picture")]
    [SwaggerOperation(
        Summary = "Delete profile picture",
        Description = "Deletes the signed-in user's profile picture."
    )]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        await _userService.DeleteProfilePicture(CurrentUserId);
        return NoContent();
    }
}