using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.Teacher;

[Route("api/teacher")]
public abstract class BaseTeacherController : BaseController
{
    protected BaseTeacherController(IMapper mapper)
        : base(mapper)
    {
    }
}