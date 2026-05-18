using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Authentication.Policies;

/// <summary>
/// This class is used to check if the user is an Student
/// </summary>
[ExcludeFromCodeCoverage]
public class IsStudentRequirement : IAuthorizationRequirement
{
    public string RoleOfStudent { get; }

    public IsStudentRequirement(string roleOfStudent)
    {
        RoleOfStudent = roleOfStudent;
    }
}

public class IsStudentHandler : AuthorizationHandler<IsStudentRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsStudentRequirement requirement)
    {
        if (context.User.IsInRole(requirement.RoleOfStudent))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}