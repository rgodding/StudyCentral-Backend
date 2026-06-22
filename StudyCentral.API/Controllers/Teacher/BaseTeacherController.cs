using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.Teacher;

[Authorize(Roles = "Teacher")]
[ApiExplorerSettings(GroupName = "teacher")]
public abstract class BaseTeacherController : BaseController
{
}
