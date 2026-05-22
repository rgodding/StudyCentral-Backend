using StudyCentral.API.Models.DtoModels;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.ApiModels.AuthModels;

public class GetUserInfoResponseModel
{
    public UserDto User { get; set; } = null!;
    public ICollection<CourseDto> Courses { get; set; } = new List<CourseDto>();
}