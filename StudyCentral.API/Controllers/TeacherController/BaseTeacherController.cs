using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using StudyCentral.API.Authentication;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.TeacherController;

[Authorize(Policy = "IsTeacher")]
public class BaseTeacherController : BaseController
{
    
    protected readonly JwtHelper _jwtHelper;
    private UserPrincipal? _userPrincipal;
    protected readonly ITeacherService _teacherService;
    
    public BaseTeacherController(IMapper mapper, ITeacherService teacherService) : base(mapper)
    {
        _teacherService = teacherService;
    }
    
    protected virtual UserPrincipal UserPrincipal
    {
        get
        {
            if (_userPrincipal == null)
            {
                _userPrincipal = JwtHelper.GetUser(User);
            }

            return _userPrincipal;
        }
    }
}