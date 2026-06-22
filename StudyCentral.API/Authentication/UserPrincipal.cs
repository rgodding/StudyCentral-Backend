using System.Diagnostics.CodeAnalysis;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Authentication;

[ExcludeFromCodeCoverage]
public class UserPrincipal
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public UserRole RoleEnum
    {
        get
        {
            if (Enum.TryParse<UserRole>(Role, ignoreCase: true, out var role))
                return role;

            throw new UnauthorizedAccessException($"Invalid user role: {Role}");
        }
    }
}