using AutoMapper;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers;

public class BaseUserController : BaseController
{
    public BaseUserController(IMapper mapper) : base(mapper)
    {
    }
    
    protected UserPrincipal UserPrincipal
    {
        get
        {
            var currentlyLoggedUser = JwtHelper.GetUser(this.User);
            return currentlyLoggedUser;
        }
    }
}