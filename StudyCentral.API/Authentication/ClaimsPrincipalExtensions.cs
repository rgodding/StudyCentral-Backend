using System.Security.Claims;

namespace StudyCentral.API.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static UserPrincipal GetUser(this ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FindFirst(ClaimTypes.Email)?.Value;
        var role = claims.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(id))
            throw new Exception("User id claim missing");

        return new UserPrincipal
        {
            Id = Guid.Parse(id),
            Email = email ?? string.Empty,
            Role = role ?? string.Empty
        };
    }
}