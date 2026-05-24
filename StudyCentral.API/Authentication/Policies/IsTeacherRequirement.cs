using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace StudyCentral.API.Authentication.Policies;

/// <summary>
/// This class is used to check if the user is an Teacher
/// </summary>
[ExcludeFromCodeCoverage]
public class IsTeacherRequirement : IAuthorizationRequirement
{
    public string RoleOfTeacher { get; }

    public IsTeacherRequirement(string roleOfTeacher)
    {
        RoleOfTeacher = roleOfTeacher;
    }
}

public class IsTeacherHandler : AuthorizationHandler<IsTeacherRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsTeacherRequirement requirement)
    {
        Console.WriteLine("Verifying if the user is a teacher");
        if (context.User.IsInRole(requirement.RoleOfTeacher))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}