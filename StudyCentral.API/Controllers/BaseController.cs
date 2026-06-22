using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Controllers;

[ApiExplorerSettings(GroupName = "public")]
public abstract class BaseController : ControllerBase
{
    protected UserPrincipal CurrentUser => User.GetUser();

    protected Guid CurrentUserId => CurrentUserId;
    protected string CurrentUserEmail => CurrentUser.Email;
    protected UserRole CurrentUserRole => CurrentUser.RoleEnum;
}