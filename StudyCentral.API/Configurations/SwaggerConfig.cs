using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace StudyCentral.API.Configurations;

public static class SwaggerConfig
{
    public static void Configure(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "StudyCentral API",
                Version = "v1"
            });

            c.SwaggerDoc("student", new OpenApiInfo
            {
                Title = "StudyCentral Student API",
                Version = "v1"
            });

            c.SwaggerDoc("teacher", new OpenApiInfo
            {
                Title = "StudyCentral Teacher API",
                Version = "v1"
            });

            c.SwaggerDoc("admin", new OpenApiInfo
            {
                Title = "StudyCentral Admin API",
                Version = "v1"
            });
            
            c.SwaggerDoc("test", new OpenApiInfo
            {
                Title = "StudyCentral Test API",
                Version = "v1"
            });

            c.EnableAnnotations();

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName == "v1")
                    return true;

                return apiDesc.GroupName == docName;
            });
        });
    }
}