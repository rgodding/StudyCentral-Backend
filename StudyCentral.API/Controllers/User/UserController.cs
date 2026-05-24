using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.Dtos.Users;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.User;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseUserController
{
    private readonly IUserService _userService;

    public UserController(IMapper mapper, IUserService userService) : base(mapper)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userService.GetCurrentUser(UserPrincipal.Id);

        return Ok(user);
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateCurrentUser(
        UpdateUserDto dto)
    {
        var user = await _userService.UpdateCurrentUser(
            UserPrincipal.Id,
            dto);

        return Ok(user);
    }
    
    [HttpPost]
    [Route("upload-profile-picture")]
    public async Task<ActionResult<UserDto>> UploadProfilePicture(IFormFile file, [FromForm] string? altText)
    {
        var user = await _userService.UpdateProfilePicture(
            UserPrincipal.Id,
            file,
            altText);
        return Ok(user);
    }
    
}