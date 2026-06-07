

using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.Admin.User;

public class AdminUpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public UserRole? Role { get; set; }
}