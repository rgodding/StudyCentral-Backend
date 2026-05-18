using AutoMapper;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers;

public class BaseUserController : BaseController
{
    protected readonly JwtHelper _jwtHelper;
    private UserPrincipal? _userPrincipal;

    public BaseUserController(IMapper mapper, JwtHelper jwtHelper) : base(mapper)
    {
        _jwtHelper = jwtHelper;
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