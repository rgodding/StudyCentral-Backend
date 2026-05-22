using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/[controller]")]
public class TeacherController : BaseTeacherController
{
    public TeacherController(IMapper mapper) : base(mapper)
    {
    }
    
}