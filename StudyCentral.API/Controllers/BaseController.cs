using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;
    
    public BaseController(IMapper mapper)
    {
        Mapper = mapper;
    }
    
    protected UserPrincipal CurrentUser => User.GetUser();
}