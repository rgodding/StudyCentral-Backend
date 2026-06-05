using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Controllers.Teacher;

[Authorize(Roles = "Teacher")]
public abstract class BaseTeacherController : BaseController
{
    protected BaseTeacherController(IMapper mapper)
        : base(mapper)
    {
    }
}