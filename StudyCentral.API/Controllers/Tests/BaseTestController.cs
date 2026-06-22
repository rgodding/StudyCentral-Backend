using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models;

namespace StudyCentral.API.Controllers.Tests;

[ApiExplorerSettings(GroupName = "test")]
public abstract class BaseTestController : BaseController
{
    protected readonly StudyDbContext _dbContext;

    protected BaseTestController(StudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}