using Microsoft.Extensions.Configuration;
using StudyCentral.API.Authentication;

namespace StudyCentral.Test.Generators;

public static class JwtServiceGenerator
{
    public static JwtService GetJwtService()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JWT:Key", "SZBNheG6DYChL2oyIo6Q3dAiK4sREZGPX6orWfH2Mk=" },
                { "JWT:Issuer", "StudyCentral.dk" },
                { "JWT:Audience", "StudyCentral.dk" },
                { "JWT:ExpirationDays", "30" }
            })
            .Build();

        return new JwtService(config);
    }
}