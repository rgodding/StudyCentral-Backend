

using StudyCentral.API.Models.DTOs.User;

namespace StudyCentral.API.Models.ApiModels.Auth;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}