using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Configurations;
using StudyCentral.API.Middleware;
using StudyCentral.API.Models;

namespace StudyCentral.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Get necessary configuration values for the application

        var issuer = Configuration["Jwt:Issuer"];
        var audience = Configuration["Jwt:Audience"];
        var key = Configuration["Jwt:Key"];

        var connectionString = Configuration["ConnectionStrings:DefaultConnection"];

        // Validate configuration values
        if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
        {
            Console.WriteLine("Invalid Configuration Values");
            throw new Exception("Invalid Configuration Values");
        }


        AuthenticationConfig.Configure(services, issuer, audience, key);
        ServiceConfig.Configure(services);
        DatabaseConfig.Configure(services, connectionString);
        SwaggerConfig.Configure(services);

        Console.WriteLine("addubg controlers");
        services.AddControllers();

        // Adds cors policy which allows any origin, method and header
        services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StudyDbContext dbContext)
    {
        dbContext.Database.Migrate();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "studycentral-backend v1");
                c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<TestRunMiddleware>();
        app.UseCors("Frontend");

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}