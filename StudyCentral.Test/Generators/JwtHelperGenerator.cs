using Microsoft.Extensions.Configuration;
using StudyCentral.API.Authentication;

namespace StudyCentral.Test.Generators;

public static class JwtHelperGenerator
{
    public static JwtHelper GetJwtHelper()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Key", "SZBNheG6DYChL2oyIo6Q3dAiK4sREZGPX6orWfH2Mk=" },
                { "Jwt:Issuer", "StudyCental.dk" },
                { "Jwt:Audience", "StudyCentral.dk" }
            }!)
            .Build();

        return new JwtHelper(config);
    }
}