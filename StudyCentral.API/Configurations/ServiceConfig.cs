using System.Text.Json.Serialization;
using StudyCentral.API.Authentication;
using StudyCentral.API.Models;
using StudyCentral.API.Services;

namespace StudyCentral.API.Configurations;

public class ServiceConfig
{
    public static void Configure(IServiceCollection services)
    {
        ServiceClasses(services);
        JsonConfig(services);
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
        services.AddScoped<IUserService, UserService>();
        

    }
    
    private static void JsonConfig(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });
    }
}