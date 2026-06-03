using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
