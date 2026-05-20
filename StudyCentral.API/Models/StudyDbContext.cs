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
        // User
        modelBuilder.Entity<User>()
            .HasOne(u => u.Avatar);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        // User & Avatar
        modelBuilder.Entity<User>()
            .HasOne(u => u.Avatar);
        
        // Course
        
        modelBuilder.Entity<Course>()
            .HasIndex(c => c.Title)
            .IsUnique();
        
        // Course & Teacher
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher);
        
        // Course & Students
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Courses)
            .UsingEntity(j => j.ToTable("CourseStudent"));
        
        // Course & Assignments
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Assignments)
            .WithOne(a => a.Course)
            .HasForeignKey(a => a.CourseId);

        // Assignment
        modelBuilder.Entity<Assignment>()
            .HasIndex(a => a.Title)
            .IsUnique();
        
        // Assignment & Files
        modelBuilder.Entity<Assignment>()
            .HasMany(a => a.Files);
        
        // Assignment & Submissions
        modelBuilder.Entity<Assignment>()
            .HasMany(a => a.Submissions)
            .WithOne(s => s.Assignment)
            .HasForeignKey(s => s.AssignmentId);
        
        // Submission
        modelBuilder.Entity<Submission>()   
            .HasIndex(s => new { s.AssignmentId, s.UserId })
            .IsUnique();
        
        // Submission & Files
        modelBuilder.Entity<Submission>()
            .HasMany(s => s.Files);
        
        // Chat
        modelBuilder.Entity<Chat>()
            .HasIndex(c => c.Name)
            .IsUnique();
        
        // Chat & Course
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Course);
        
        // Chat & Messages
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(m => m.ChatId);
        
        // Chat & Participants
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Participants)
            .WithMany(p => p.Chats)
            .UsingEntity(j => j.ToTable("ChatParticipant"));
        
        // Message
        
        // Message & User
        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId);
       
        // Notification
        modelBuilder.Entity<Notification>()
            .HasIndex(n => n.Title)
            .IsUnique();
        
        // User & Notifications
        modelBuilder.Entity<User>()
            .HasMany(u => u.Notifications)
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId);
        
        
        // SEED DATA
        
    }
    
}