using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Controllers.Student;

[Authorize(Policy = "IsStudent")]
public class BaseStudentController : BaseUserController
{
    public BaseStudentController(IMapper mapper) : base(mapper)
    {
    }
}