namespace StudyCentral.API.Models.Dtos.Users;

public class UserDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Role { get; set; } = null!;
}