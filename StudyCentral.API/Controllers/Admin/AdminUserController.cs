using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.User;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
public class AdminUserController : BaseAdminController
{
    private readonly IUserService _userService;

    public AdminUserController(
        IMapper mapper,
        IUserService userService)
        : base(mapper)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _userService.GetAll();

        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(
        Guid userId)
    {
        var user = await _userService.GetById(userId);

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        [FromBody] CreateUserDto dto)
    {
        var user = await _userService.Create(dto);

        return CreatedAtAction(
            nameof(GetUser),
            new { userId = user.Id },
            user);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<UserDto>> UpdateUser(
        Guid userId,
        [FromBody] AdminUpdateUserDto dto)
    {
        var user = await _userService.AdminUpdateUser(
            userId,
            dto);

        return Ok(user);
    }

    [HttpPut("{userId}/password")]
    public async Task<IActionResult> UpdatePassword(
        Guid userId,
        [FromBody] AdminUpdateUserPasswordDto dto)
    {
        await _userService.AdminUpdatePassword(
            userId,
            dto);

        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(
        Guid userId)
    {
        await _userService.Delete(userId);

        return NoContent();
    }
}