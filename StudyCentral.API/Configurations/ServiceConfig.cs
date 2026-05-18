using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Services;

namespace StudyCentral.API.Configurations;

public class ServiceConfig
{
    public static void Configure(IServiceCollection services)
    {
        ServiceClasses(services);
    }
    private static void ServiceClasses(IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));
        // JWT
        services.AddScoped<JwtHelper>();
        // Services
        services.AddScoped<IUserService, UserService>();

    }
}