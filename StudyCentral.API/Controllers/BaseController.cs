using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Authentication;

namespace StudyCentral.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly IMapper _mapper;
    
    public BaseController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    protected UserPrincipal CurrentUser => User.GetUser();
}