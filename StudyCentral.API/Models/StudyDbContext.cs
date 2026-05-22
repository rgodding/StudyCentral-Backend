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
    public DbSet<Announcement> Announcements { get; set; } = null!;
    public DbSet<StudyFile> Files { get; set; } = null!;
    public DbSet<ImageFile> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        // User & Avatar
        modelBuilder.Entity<User>()
            .HasOne(u => u.ProfilePicture);
        
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
        
        // Announcement
        modelBuilder.Entity<Announcement>()
            .HasIndex(a => a.Title)
            .IsUnique();
        
        // Announcement & Course
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Announcements)
            .HasForeignKey(a => a.CourseId);
        
        
        // SEED DATA
        // Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("08deb68b-5c0f-447c-86a2-152bdf58b714"),
                Email = "user@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "John",
                LastName = "Student",
                Role = UserRole.Student
            },
            new User
            {
                Id = Guid.Parse("08deb68b-c568-4182-841f-7f7f7da655d8"),
                Email = "teacher@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Jonathan",
                LastName = "Teacher",
                Role = UserRole.Teacher
            },
            new User
            {
                Id = Guid.Parse("08deb68b-ca1a-49f8-80a9-cebcdca84136"),
                Email = "admin@mail.com",
                PasswordHash = "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
                FirstName = "Mister",
                LastName = "Admin",
                Role = UserRole.Admin
            }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course
            {

                Id = Guid.Parse("08deb82b-8d61-4e71-8ab5-2205c9dd79ba"),
                Title = "Introduction to Programming",
                Description = "Learn the basics of programming with this introductory course.",

            },
            new Course
            {

                Id = Guid.Parse("08deb82b-9093-4baa-806d-5a095a01328f"),
                Title = "The Art of Programming",
                Description = "Explore the art of programming with this course.",

            },
            new Course
            {

                Id = Guid.Parse("08deb82b-9368-4f46-8c29-b0498685408c"),
                Title = "Advanced Programming",
                Description = "Master advanced programming techniques with this course.",
            }
        );


    }
    
}