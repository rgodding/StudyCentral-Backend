using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StudyCentral.API.Configurations;
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
        
        // Seed Data
        SeedData.Seed(modelBuilder);
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

        modelBuilder.Entity<User>()
            .HasMany(u => u.EnrolledCourses)
            .WithMany(c => c.Students);
    }
    
    private static void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Course>()
            .Property(c => c.Description)
            .HasMaxLength(1000);
        
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(u => u.TeachingCourses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    private static void ConfigureAssignment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>()
            .Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Assignment>()
            .Property(a => a.Description)
            .HasMaxLength(2000);
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.CreatedBy)
            .WithMany(u => u.CreatedAssignments)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    private static void ConfigureSubmission(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>()
            .Property(s => s.Feedback)
            .HasMaxLength(2000);

        modelBuilder.Entity<Submission>()
            .Property(s => s.Comment)
            .HasMaxLength(2000);
        
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Student)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Submission>()
            .HasIndex(s => new { s.AssignmentId, s.StudentId })
            .IsUnique();
    }

    private static void ConfigureAnnouncement(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>()
            .Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Announcement>()
            .Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(5000);
        
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Announcements)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.CreatedBy)
            .WithMany(u => u.CreatedAnnouncements)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureStudyFile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyFile>()
            .Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<StudyFile>()
            .Property(f => f.BlobName)
            .IsRequired()
            .HasMaxLength(500);

        modelBuilder.Entity<StudyFile>()
            .Property(f => f.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<StudyFile>()
            .Property(f => f.AltText)
            .HasMaxLength(100);
        
        // Uploader
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.UploadedBy)
            .WithMany(u => u.UploadedFiles)
            .HasForeignKey(f => f.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Course
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.Course)
            .WithMany(c => c.Files)
            .HasForeignKey(f => f.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Assignment
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.Assignment)
            .WithMany(a => a.Files)
            .HasForeignKey(f => f.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Submission
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.Submission)
            .WithMany(s => s.Files)
            .HasForeignKey(f => f.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}