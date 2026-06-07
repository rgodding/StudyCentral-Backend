using System.Diagnostics.CodeAnalysis;

namespace StudyCentral.API.Authentication;

[ExcludeFromCodeCoverage]
public class UserPrincipal
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}