using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace StudyCentral.API.Authentication;

[ExcludeFromCodeCoverage]
public static class ClaimsPrincipalExtensions
{
    public static UserPrincipal GetUser(this ClaimsPrincipal claims)
    {
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? throw new UnauthorizedAccessException("User id claim missing");

        return new UserPrincipal
        {
            Id = Guid.Parse(id),
            Email = claims.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Role = claims.FindFirstValue(ClaimTypes.Role) ?? string.Empty
        };
    }
}