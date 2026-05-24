using StudyCentral.API.Models.Dtos.Users;

namespace StudyCentral.API.Models.Dtos.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}