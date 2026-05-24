using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudyCentral.API.Models.DtoModels;
using StudyCentral.API.Services;

namespace StudyCentral.API.Controllers.PublicController;

[ApiController]
[Route("api/[controller]")]
public class TestController : BaseController
{
    
    private readonly IBlobService _blobService;
    private readonly IUserService _userService;
    private readonly ITeacherService _teacherService;
    private readonly ICourseService _courseService;
    private readonly IStudentService _studentService;
    private readonly IChatService _chatService;

    public TestController(IMapper mapper, IBlobService blobService, IUserService userService, ITeacherService teacherService, ICourseService courseService, IStudentService studentService, IChatService chatService) : base(mapper)
    {
        _blobService = blobService;
        _userService = userService;
        _teacherService = teacherService;
        _courseService = courseService;
        _studentService = studentService;
        _chatService = chatService;
    }
    
    
    [HttpGet]
    [Route("get-users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsers();
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        return Ok(userDtos);    
    }
        
}