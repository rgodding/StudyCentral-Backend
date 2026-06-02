using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using StudyCentral.API.Controllers.User;

namespace StudyCentral.API.Controllers.Teacher;

[Authorize(Policy = "IsTeacher")]
public class BaseTeacherController : BaseUserController
{
    public BaseTeacherController(IMapper mapper) : base(mapper)
    {
    }
}