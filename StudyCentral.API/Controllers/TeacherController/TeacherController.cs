using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : BaseTeacherController
{
    public TeacherController(IMapper mapper, ITeacherService teacherService) : base(mapper, teacherService)
    {
    }
}