using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.User;

public class UserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public UserRole Role { get; set; }

    public string? ProfilePictureUrl { get; set; }
}