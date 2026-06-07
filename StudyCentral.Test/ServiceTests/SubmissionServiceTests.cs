using System.Security;
using Microsoft.EntityFrameworkCore;
using StudyCentral.API.Models;
using StudyCentral.API.Models.DTOs.Submission;
using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Relationship;
using StudyCentral.API.Services;
using StudyCentral.Test.Factories;
using StudyCentral.Test.Generators;
using Xunit;

namespace StudyCentral.Test.ServiceTests;

public class SubmissionServiceTests
{
    private readonly StudyDbContext _dbContext;
    private readonly SubmissionService _service;

    public SubmissionServiceTests()
    {
        _dbContext = ContextGenerator.GetStudyDbContext();
        var mapper = MapperGenerator.GetMapper();
        IStudyFileService studyFileService = new StudyFileServiceGenerator();
        _service = new SubmissionService(_dbContext, mapper, studyFileService);
    }

    // ----------------
    // GetAll
    // ----------------

    [Fact]
    public async Task GetAll_WhenSubmissionsExist_ReturnsAllSubmissions()
    {
        // Arrange
        var assignment = TestAssignmentFactory.Create();
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();

        var submissions = new List<Submission>
        {
            new Submission
            {
                Id = Guid.NewGuid(),
                AssignmentId = assignment.Id,
                StudentId = student.Id,
                Comment = "Test 1",
                SubmittedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            },
            new Submission
            {
                Id = Guid.NewGuid(),
                AssignmentId = assignment.Id,
                StudentId = student.Id,
                Comment = "Test 2",
                SubmittedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            }
        };

        _dbContext.AddRange(submissions);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAll_WhenNoSubmissions_ReturnsEmptyList()
    {
        // Act
        var result = await _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    // ----------------
    // GetById
    // ----------------

    [Fact]
    public async Task GetById_WhenSubmissionExists_ReturnsSubmission()
    {
        // Arrange
        var assignment = TestAssignmentFactory.Create();
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "Test submission",
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Add(submission);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.GetById(submission.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(submission.Id, result.Id);
        Assert.Equal(submission.Comment, result.Comment);
    }

    [Fact]
    public async Task GetById_WhenSubmissionDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetById(id));
    }

    // ----------------
    // Create
    // ----------------

    [Fact]
    public async Task Create_WhenValidData_ReturnsCreatedSubmission()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };
        
        assignment.CourseId = course.Id;
        var studentEnrollment = new CourseStudent
        {
            CourseId = course.Id,
            StudentId = student.Id
        };
        _dbContext.Add(studentEnrollment);
        _dbContext.Add(course);
        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateSubmissionDto
        {
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "New submission"
        };

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Comment, result.Comment);
        Assert.Equal(dto.StudentId, result.StudentId);
    }

    [Fact]
    public async Task Create_WhenAssignmentDontExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dto = new CreateSubmissionDto
        {
            AssignmentId = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            Comment = "New submission"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Create(dto));
    }
    
    [Fact]
    public async Task Create_WhenStudentDontExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        _dbContext.Add(course);
        _dbContext.Add(assignment);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateSubmissionDto
        {
            AssignmentId = assignment.Id,
            StudentId = Guid.NewGuid(),
            Comment = "New submission"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Create(dto));
    }

    [Fact]
    public async Task Create_WhenUserIsNotStudent_ThrowsInvalidOperationException()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        var teacher = TestUserFactory.Create(role: UserRole.Teacher);
        
        _dbContext.Add(course);
        _dbContext.Add(assignment);
        _dbContext.Add(teacher);
        await _dbContext.SaveChangesAsync();
        
        var dto = new CreateSubmissionDto
        {
            AssignmentId = assignment.Id,
            StudentId = teacher.Id,
            Comment = "New submission"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.Create(dto));
    }
    
    [Fact]
    public async Task Create_WhenUserIsNotEnrolledInCourse_ThrowsInvalidOperationException()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        var student = TestUserFactory.Create(role: UserRole.Student);
        
        _dbContext.Add(course);
        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();
        
        var dto = new CreateSubmissionDto
        {
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "New submission"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<SecurityException>(() =>
            _service.Create(dto));
    }
    
    [Fact]
    public async Task Create_WhenSubmissionAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        var student = TestUserFactory.Create(role: UserRole.Student);
        
        assignment.CourseId = course.Id;
        var studentEnrollment = new CourseStudent
        {
            CourseId = course.Id,
            StudentId = student.Id
        };
        
        _dbContext.Add(studentEnrollment);
        _dbContext.Add(course);
        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "Existing submission",
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Add(submission);
        await _dbContext.SaveChangesAsync();

        var dto = new CreateSubmissionDto
        {
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "New submission"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.Create(dto));
    }

    // ----------------
    // Update
    // ----------------

    [Fact]
    public async Task Update_WhenSubmissionExists_ReturnsUpdatedSubmission()
    {
        // Arrange
        var assignment = TestAssignmentFactory.Create();
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "Old comment",
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Add(submission);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateSubmissionDto
        {
            Comment = "Updated comment"
        };

        // Act
        var result = await _service.Update(submission.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Comment, result.Comment);
    }

    [Fact]
    public async Task Update_WhenSubmissionDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var dto = new UpdateSubmissionDto
        {
            Comment = "Updated"
        };

        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Update(id, dto));
    }
    
    [Theory]
    [InlineData("Updated")]
    [InlineData(null)]
    public async Task Update_WhenValidCombination_ReturnsUpdatedSubmission(string? newComment)
    {
        // Arrange
        var course = TestCourseFactory.Create();
        var assignment = TestAssignmentFactory.Create(courseId: course.Id);
        var student = TestUserFactory.Create(role: UserRole.Student);
        var submission = TestSubmissionFactory.Create(assignmentId: assignment.Id, studentId: student.Id);
        
        _dbContext.Add(course);
        _dbContext.Add(assignment);
        _dbContext.Add(student);
        _dbContext.Add(submission);
        await _dbContext.SaveChangesAsync();

        var dto = new UpdateSubmissionDto
        {
            Comment = newComment
        };

        // Act
        var result = await _service.Update(submission.Id, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newComment, result.Comment);
    }

    // ----------------
    // Delete
    // ----------------

    [Fact]
    public async Task Delete_WhenSubmissionExists_DeletesSubmission()
    {
        // Arrange
        var assignment = TestAssignmentFactory.Create();
        var student = new User
        {
            Id = Guid.NewGuid(),
            Email = "student@test.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hash",
            Role = UserRole.Student
        };

        _dbContext.Add(assignment);
        _dbContext.Add(student);
        await _dbContext.SaveChangesAsync();

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            AssignmentId = assignment.Id,
            StudentId = student.Id,
            Comment = "To delete",
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Add(submission);
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.Delete(submission.Id);

        // Assert
        var exists = await _dbContext.Submissions
            .AnyAsync(s => s.Id == submission.Id);

        Assert.False(exists);
    }

    [Fact]
    public async Task Delete_WhenSubmissionDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.Delete(id));
    }
}