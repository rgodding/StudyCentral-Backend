using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/[controller]")]
public class TeacherCourseController : BaseTeacherController
{
    public TeacherCourseController(IMapper mapper) : base(mapper)
    {
    }
}