using System.Text.Json.Serialization;
using StudyCentral.API.Authentication;
using StudyCentral.API.Data.Seed;
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
        
        // SignalR
        services.AddSignalR();

        // JWT
        services.AddScoped<IJwtService, JwtService>();

        // Blob
        services.AddSingleton<IBlobService, BlobService>();

        // File Services
        services.AddScoped<IStudyFileService, StudyFileService>();
        
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<IStudyFolderService, StudyFolderService>();
        services.AddScoped<IStudyFileService, StudyFileService>();
        services.AddScoped<IChatService, ChatService>();
        
        // Test Data Seed
        services.AddScoped<ISchoolDemoDataSeeder, SchoolDemoDataSeeder>();
        services.AddScoped<ICreateTestDataService, CreateTestDataService>();
    }

    private static void JsonConfig(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });
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
