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
        var response = await _userService.GetById(CurrentUser.Id);
        return Ok(response);
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