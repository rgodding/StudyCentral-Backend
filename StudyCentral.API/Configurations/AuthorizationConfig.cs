using Microsoft.AspNetCore.Authentication.JwtBearer;
using StudyCentral.API.Authentication.Policies;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Configurations;

public class AuthorizationConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new IsAdminRequirement(nameof(UserRole.Admin)));
            });
            options.AddPolicy("IsTeacher", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new IsTeacherRequirement(nameof(UserRole.Teacher)));
            });
            options.AddPolicy("IsStudent", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new IsStudentRequirement(nameof(UserRole.Student)));
            });
            
            options.AddPolicy("IsUser", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new IsUserRequirement(nameof(UserRole.Student), nameof(UserRole.Teacher), nameof(UserRole.Admin)));
            });
        });
    }
}