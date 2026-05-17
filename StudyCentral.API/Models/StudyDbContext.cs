using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models;

public class StudyDbContext : DbContext
{
    public StudyDbContext(DbContextOptions<StudyDbContext> options) : base(options)
    {
    }
    
    public class StudyDbContextFactory : IDesignTimeDbContextFactory<StudyDbContext>
    {
        public StudyDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<StudyDbContext>()
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string not set");
            }

            var optionsBuilder = new DbContextOptionsBuilder<StudyDbContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
            return new StudyDbContext(optionsBuilder.Options);
        }
    }
    
    // Models to be set
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<StudyFile> Files { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
    
}