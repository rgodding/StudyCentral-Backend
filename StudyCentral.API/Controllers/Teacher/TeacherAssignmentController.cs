using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.Teacher;

[ApiController]
[Route("api/teacher/[controller]")]
public class TeacherAssignmentController : BaseTeacherController
{
    
    public TeacherAssignmentController(IMapper mapper) : base(mapper)
    {
    }
}