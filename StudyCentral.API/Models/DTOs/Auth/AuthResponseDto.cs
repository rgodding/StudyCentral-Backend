using StudyCentral.API.Models.DTOs.User;

namespace StudyCentral.API.Models.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}