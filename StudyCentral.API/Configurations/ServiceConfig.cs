using Microsoft.AspNetCore.Authorization;
using StudyCentral.API.Authentication;
using StudyCentral.API.Authentication.Policies;
using StudyCentral.API.Models;
using StudyCentral.API.Services;

namespace StudyCentral.API.Configurations;

public class ServiceConfig
{
    public static void Configure(IServiceCollection services)
    {
        AuthorizationHandlers(services);
        ServiceClasses(services);
        JsonConfig(services);
    }

    private static void AuthorizationHandlers(IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, IsAdminHandler>();
        services.AddSingleton<IAuthorizationHandler, IsTeacherHandler>();
        services.AddSingleton<IAuthorizationHandler, IsStudentHandler>();
        
    }

    private static void ServiceClasses(IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));
        // JWT
        services.AddScoped<JwtHelper>();
        // Blob
        services.AddSingleton<IBlobService, BlobService>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        
    }

    private static void JsonConfig(IServiceCollection services)
    {
        /*
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });
            */
    }
}