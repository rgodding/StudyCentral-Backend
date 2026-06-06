using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Controllers.Student;

[Authorize(Roles = "Student")]
public abstract class StudentController : BaseController
{
    protected StudentController(IMapper mapper) : base(mapper)
    {
    }
}