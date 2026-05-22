using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers.AdminController;

[Authorize(Policy = "IsAdmin")]
public class BaseAdminController : BaseController
{
    protected readonly JwtHelper _jwtHelper;
    private UserPrincipal? _userPrincipal;


    public BaseAdminController(IMapper mapper, JwtHelper jwtHelper, UserPrincipal? userPrincipal) : base(mapper)
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