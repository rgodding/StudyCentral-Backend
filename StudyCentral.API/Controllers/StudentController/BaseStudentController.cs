using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers.StudentController;

[Authorize(Policy = "IsStudent")]
public class BaseStudentController : BaseController
{
    protected readonly JwtHelper _jwtHelper;
    private UserPrincipal? _userPrincipal;

    public BaseStudentController(IMapper mapper, JwtHelper jwtHelper, UserPrincipal? userPrincipal) : base(mapper)
    {
        _jwtHelper = jwtHelper;
        _userPrincipal = userPrincipal;
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