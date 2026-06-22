using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DTOs.Admin.User;
using StudyCentral.API.Models.DTOs.User;
using StudyCentral.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace StudyCentral.API.Controllers.Admin;

[ApiController]
[Tags("Admin - Users")]
[Route("api/admin/users")]
public class AdminUserController : BaseAdminController
{
    private readonly IUserService _userService;

    public AdminUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get users",
        Description = "Gets all users."
    )]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{userId:guid}")]
    [SwaggerOperation(
        Summary = "Get user",
        Description = "Gets one user by id."
    )]
    public async Task<ActionResult<UserDto>> GetUser(Guid userId)
    {
        var user = await _userService.GetById(userId);
        return Ok(user);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create user",
        Description = "Creates a new user."
    )]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
    {
        var user = await _userService.Create(dto);

        return CreatedAtAction(
            nameof(GetUser),
            new { userId = user.Id },
            user);
    }

    [HttpPut("{userId:guid}")]
    [SwaggerOperation(
        Summary = "Update user",
        Description = "Updates one user by id."
    )]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid userId, [FromBody] AdminUpdateUserDto dto)
    {
        var user = await _userService.AdminUpdateUser(userId, dto);
        return Ok(user);
    }

    [HttpPut("{userId:guid}/password")]
    [SwaggerOperation(
        Summary = "Update password",
        Description = "Updates one user's password."
    )]
    public async Task<IActionResult> UpdatePassword(Guid userId, [FromBody] AdminUpdateUserPasswordDto dto)
    {
        await _userService.AdminUpdatePassword(userId, dto);
        return NoContent();
    }

    [HttpDelete("{userId:guid}")]
    [SwaggerOperation(
        Summary = "Delete user",
        Description = "Deletes one user by id."
    )]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.Delete(userId);
        return NoContent();
    }
}