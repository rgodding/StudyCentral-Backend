using AutoMapper;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers.TeacherController;

public class BaseTeacherController : BaseController
{
    
    protected readonly JwtHelper _jwtHelper;
    private UserPrincipal? _userPrincipal;
    
    public BaseTeacherController(IMapper mapper) : base(mapper)
    {
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