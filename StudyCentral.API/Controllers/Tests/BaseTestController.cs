using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models;

namespace StudyCentral.API.Controllers.Tests;

[ApiExplorerSettings(GroupName = "test")]
[Tags("Test")]
[Route("api/public/test")]
public abstract class BaseTestController : BaseController
{
    protected readonly StudyDbContext _dbContext;
    
    protected BaseTestController(IMapper mapper, StudyDbContext dbContext) : base(mapper)
    {
        _dbContext = dbContext;
    }
}