using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/[controller]")]
public class TeacherAssignmentController : BaseTeacherController
{
    public TeacherAssignmentController(IMapper mapper) : base(mapper)
    {
    }
}