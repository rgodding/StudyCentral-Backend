using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StudyCentral.API.Configurations;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Relationship;

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
    public DbSet<StudyFolder> StudyFolders { get; set; } = null!;
    public DbSet<StudyFile> StudyFiles { get; set; } = null!;
    public DbSet<CourseStudent> CourseStudents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entities
        ConfigureUser(modelBuilder);
        ConfigureCourse(modelBuilder);
        ConfigureAssignment(modelBuilder);
        ConfigureSubmission(modelBuilder);
        ConfigureAnnouncement(modelBuilder);
        ConfigureStudyFolder(modelBuilder);
        ConfigureStudyFile(modelBuilder);
        ConfigureCourseStudent(modelBuilder);

        // Chat Configurations
        ConfigureChatRoom(modelBuilder);
        ConfigureChatMessage(modelBuilder);
        ConfigureChatRoomMember(modelBuilder);

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
            .WithOne(cs => cs.Student)
            .HasForeignKey(cs => cs.StudentId);
    }

    private static void ConfigureCourse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(u => u.TeachingCourses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Course>()
            .HasMany(c => c.CourseStudents)
            .WithOne(cs => cs.Course)
            .HasForeignKey(cs => cs.CourseId);
    }

    private static void ConfigureAssignment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>()
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Assignment>()
            .Property(a => a.Description)
            .HasMaxLength(2000);

        modelBuilder.Entity<Assignment>()
            .HasIndex(a => new
            {
                a.CourseId,
                a.Name
            })
            .IsUnique();

        // Many-to-one
        // An assignment belongs to one course
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureSubmission(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>()
            .Property(s => s.Comment)
            .HasMaxLength(2000);

        modelBuilder.Entity<Submission>()
            .Property(s => s.Feedback)
            .HasMaxLength(2000);

        // Many-to-one
        // A submission belongs to one assignment
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-one
        // A submission belongs to one student
        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Student)
            .WithMany(u => u.Submissions)
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique submission per student per assignment
        modelBuilder.Entity<Submission>()
            .HasIndex(s => new { s.AssignmentId, s.StudentId })
            .IsUnique();
    }

    private static void ConfigureAnnouncement(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>()
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Announcement>()
            .Property(a => a.Content)
            .IsRequired()
            .HasMaxLength(5000);

        // Many-to-one
        // An announcement belongs to one course
        modelBuilder.Entity<Announcement>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Announcements)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureStudyFolder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyFolder>()
            .Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Many-to-one
        // A folder belongs to one course
        modelBuilder.Entity<StudyFolder>()
            .HasOne(f => f.Course)
            .WithMany(c => c.StudyFolders)
            .HasForeignKey(f => f.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-one
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.StudyFolder)
            .WithMany(f => f.StudyFiles)
            .HasForeignKey(f => f.StudyFolderId);

        // Self-referencing one-to-many
        // A folder can contain many child folders
        modelBuilder.Entity<StudyFolder>()
            .HasOne(f => f.ParentFolder)
            .WithMany(f => f.ChildFolders)
            .HasForeignKey(f => f.ParentFolderId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureStudyFile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyFile>()
            .Property(f => f.FileName)
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
            .HasMaxLength(500);

        // Uploader
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.UploadedBy)
            .WithMany(u => u.UploadedFiles)
            .HasForeignKey(f => f.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Folder
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.StudyFolder)
            .WithMany(f => f.StudyFiles)
            .HasForeignKey(f => f.StudyFolderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Assignment
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.Assignment)
            .WithMany(a => a.StudyFiles)
            .HasForeignKey(f => f.AssignmentId)
            .OnDelete(DeleteBehavior.SetNull);

        // Announcement
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.Announcement)
            .WithMany(a => a.StudyFiles)
            .HasForeignKey(f => f.AnnouncementId)
            .OnDelete(DeleteBehavior.SetNull);

        // Submission
        modelBuilder.Entity<StudyFile>()
            .HasOne(f => f.Submission)
            .WithMany(s => s.StudyFiles)
            .HasForeignKey(f => f.SubmissionId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    private static void ConfigureCourseStudent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseStudent>(entity =>
        {
            entity.ToTable("CourseStudent");

            entity.HasKey(cs => new { cs.CourseId, cs.StudentId });

            entity.Property(cs => cs.CourseId).IsRequired();
            entity.Property(cs => cs.StudentId).IsRequired();

            entity.HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId);

            entity.HasOne(cs => cs.Student)
                .WithMany(u => u.EnrolledCourses)
                .HasForeignKey(cs => cs.StudentId);
        });
    }

    private static void ConfigureChatRoom(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatRoom>()
            .HasOne(x => x.Course)
            .WithMany()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureChatMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatMessage>()
            .HasOne(x => x.ChatRoom)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureChatRoomMember(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatRoomMember>()
            .HasKey(x => new { x.UserId, x.ChatRoomId });

        modelBuilder.Entity<ChatRoomMember>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatRoomMember>()
            .HasOne(x => x.ChatRoom)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}