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
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<Announcement> Announcements { get; set; } = null!;
    public DbSet<StudyFile> Files { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entities
        ConfigureUser(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureAssignment(modelBuilder);
        ConfigureSubmission(modelBuilder);
        ConfigureAnnouncement(modelBuilder);
        ConfigureStudyFile(modelBuilder);
        
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
                TeacherId = Guid.Parse("08deb68b-c568-4182-841f-7f7f7da655d8"),

            },
            new Course
            {

                Id = Guid.Parse("08deb82b-9093-4baa-806d-5a095a01328f"),
                Title = "The Art of Programming",
                Description = "Explore the art of programming with this course.",
                TeacherId = Guid.Parse("08deb68b-c568-4182-841f-7f7f7da655d8"),
            },
            new Course
            {

                Id = Guid.Parse("08deb82b-9368-4f46-8c29-b0498685408c"),
                Title = "Advanced Programming",
                Description = "Master advanced programming techniques with this course.",
            }
        );


    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.ProfilePicture)
            .WithMany()
            .HasForeignKey(u => u.ProfilePictureId)
            .OnDelete(DeleteBehavior.SetNull);

        // Student enrollments
        modelBuilder.Entity<User>()
            .HasMany(u => u.EnrolledCourses)
            .WithMany(c => c.Students);

        // Teacher courses
        modelBuilder.Entity<User>()
            .HasMany(u => u.TeachingCourses)
            .WithOne(c => c.Teacher)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        // Created assignments
        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedAssignments)
            .WithOne(a => a.CreatedBy)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Created announcements
        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedAnnouncements)
            .WithOne(a => a.CreatedBy)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Submissions
        modelBuilder.Entity<User>()
            .HasMany(u => u.Submissions)
            .WithOne(s => s.Student)
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    private static void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .Property(c => c.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<Course>()
            .Property(c => c.Description)
            .HasMaxLength(1000);
    }
    
    private static void ConfigureAssignment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Assignment>()
            .Property(a => a.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<Assignment>()
            .Property(a => a.Description)
            .HasMaxLength(2000);
    }
    
    private static void ConfigureSubmission(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Submission>()
            .Property(s => s.Feedback)
            .HasMaxLength(2000);

        modelBuilder.Entity<Submission>()
            .Property(s => s.Comment)
            .HasMaxLength(2000);
    }
    
    private static void ConfigureAnnouncement(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Announcements)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Announcement>()
            .Property(a => a.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<Announcement>()
            .Property(a => a.Content)
            .HasMaxLength(5000);
    }
    
    private static void ConfigureStudyFile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyFile>()
            .Property(f => f.Name)
            .HasMaxLength(255);

        modelBuilder.Entity<StudyFile>()
            .Property(f => f.BlobName)
            .HasMaxLength(500);

        modelBuilder.Entity<StudyFile>()
            .Property(f => f.ContentType)
            .HasMaxLength(100);

        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.UploadedBy)
            .WithMany()
            .HasForeignKey(f => f.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
}