using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IMapper _mapper;
    
    public BaseController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
}