namespace StudyCentral.API.Authentication;

public class UserPrincipal
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}