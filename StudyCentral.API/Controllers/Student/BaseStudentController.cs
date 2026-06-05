using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Controllers.Student;

[Authorize(Roles = "Student")]
public abstract class BaseStudentController : BaseController
{
    protected BaseStudentController(IMapper mapper)
        : base(mapper)
    {
    }
}