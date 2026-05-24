using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.TeacherController;

[ApiController]
[Route("api/[controller]")]
public class TeacherAnnouncementController : BaseTeacherController
{
    public TeacherAnnouncementController(IMapper mapper, ITeacherService teacherService) : base(mapper, teacherService)
    {
    }
}