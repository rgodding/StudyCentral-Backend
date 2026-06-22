using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Controllers;

[ApiExplorerSettings(GroupName = "public")]
public abstract class BaseController : ControllerBase
{
    protected UserPrincipal CurrentUser => User.GetUser();

    protected Guid CurrentUserId => CurrentUser.Id;

    protected string CurrentUserEmail => CurrentUser.Email;

    protected UserRole CurrentUserRole
    {
        get
        {
            if (Enum.TryParse<UserRole>(CurrentUser.Role, ignoreCase: true, out var role))
                return role;

            throw new UnauthorizedAccessException($"Invalid user role: {CurrentUser.Role}");
        }
    }
}