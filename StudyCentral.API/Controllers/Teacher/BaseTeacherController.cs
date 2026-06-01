using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Controllers.Teacher;

[Authorize(Policy = "IsTeacher")]
public class BaseTeacherController : BaseUserController
{
    public BaseTeacherController(IMapper mapper) : base(mapper)
    {
    }
}