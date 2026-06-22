using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.Admin;

[Authorize(Roles = "Admin")]
[ApiExplorerSettings(GroupName = "admin")]
public abstract class BaseAdminController : BaseController
{
}