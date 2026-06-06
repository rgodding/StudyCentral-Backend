using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.Student;

[Authorize(Roles = "Student")]
[ApiExplorerSettings(GroupName = "student")]
public abstract class BaseStudentController : BaseController
{
    protected BaseStudentController(IMapper mapper)
        : base(mapper)
    {
    }
}