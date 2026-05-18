using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Authentication.Policies;

/// <summary>
/// This class is used to check if the user is an admin
/// </summary>
[ExcludeFromCodeCoverage]
public class IsAdminRequirement : IAuthorizationRequirement
{
    public string RoleOfAdmin { get; }

    public IsAdminRequirement(string roleOfAdmin)
    {
        RoleOfAdmin = roleOfAdmin;
    }
}

public class IsAdminHandler : AuthorizationHandler<IsAdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
    {
        if (context.User.IsInRole(requirement.RoleOfAdmin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}