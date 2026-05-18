using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;

namespace StudyCentral.API.Configurations;

public static class DatabaseConfig
{
    public static void Configure(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StudyDbContext>(options =>
            options.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 29))));
    }
}