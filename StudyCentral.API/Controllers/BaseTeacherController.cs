using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Controllers;

[Authorize(Policy = "IsTeacher")]
public class BaseTeacherController : BaseUserController
{
    public BaseTeacherController(IMapper mapper) : base(mapper)
    {
    }
}