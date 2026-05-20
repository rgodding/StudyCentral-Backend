using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Authentication.Policies;

/// <summary>
/// This class is used to check if User
/// </summary>
[ExcludeFromCodeCoverage]
public class IsUserRequirement : IAuthorizationRequirement
{
    public string RoleOfAdmin { get; }
    public string RoleOfTeacher { get; }
    public string RoleOfStudent { get; }

    public IsUserRequirement(string roleOfUser, string roleOfTeacher, string roleOfStudent)
    {
        RoleOfAdmin = roleOfUser;
        RoleOfTeacher = roleOfTeacher;
        RoleOfStudent = roleOfStudent;
    }
}

public class IsUserHandler : AuthorizationHandler<IsUserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsUserRequirement requirement)
    {
        if (context.User.IsInRole(requirement.RoleOfAdmin) ||
            context.User.IsInRole(requirement.RoleOfTeacher) ||
            context.User.IsInRole(requirement.RoleOfStudent))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}