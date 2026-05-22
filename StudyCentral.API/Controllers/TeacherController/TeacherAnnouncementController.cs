using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/[controller]")]
public class TeacherAnnouncementController : BaseTeacherController
{
    public TeacherAnnouncementController(IMapper mapper) : base(mapper)
    {
    }
}