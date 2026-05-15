using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace StudyCentral.API.Controllers;

public class BaseController : ControllerBase
{
    protected readonly IMapper Mapper;
    
    public BaseController(IMapper mapper)
    {
        Mapper = mapper;
    }
    
}