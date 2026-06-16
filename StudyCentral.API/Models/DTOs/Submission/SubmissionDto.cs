using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.DTOs.Submission;

public class SubmissionDto
{
    public Guid Id { get; set; }

    // Submission
    public DateTime SubmittedAt { get; set; }
    public string? Comment { get; set; }
    public SubmissionStatus Status { get; set; }

    // Review
    public string? Feedback { get; set; }
    public GradeLetter? Grade { get; set; }
    public DateTime? GradedAt { get; set; }

    // Assignment
    public Guid AssignmentId { get; set; }
    public string AssignmentName { get; set; } = null!;

    // Student
    public Guid StudentId { get; set; }
    public string StudentFirstName { get; set; } = null!;
    public string? StudentLastName { get; set; }
    public string? StudentProfilePictureUrl { get; set; }

    // Files
    public int FileCount { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}