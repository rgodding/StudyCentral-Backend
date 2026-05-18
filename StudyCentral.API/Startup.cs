using StudyCentral.API.Configurations;
using StudyCentral.API.Middleware;

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
        AuthorizationConfig.Configure(services);
        ServiceConfig.Configure(services);
        DatabaseConfig.Configure(services, connectionString);
        SwaggerConfig.Configure(services);

        Console.WriteLine("addubg controlers");
        services.AddControllers();
        
        // Adds cors policy which allows any origin, method and header
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Apply EF Core migrations if database doesn't exist
        
        // Allows cors policy
        app.UseCors("AllowAll");
        
        // Middleware
        
        // Is in development environment
        if (env.IsDevelopment())
        {
            // Middleware for Testing
            // app.UseMiddleware<TestRunMiddleware>();
            
            // app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "studycentral-backend v1");

                // Persist Bearer token between requests & refreshes
                c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
            });
        }

        Console.WriteLine("okay almost there");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseAuthentication();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}