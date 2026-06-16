using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.Test.Factories;

public static class TestSubmissionFactory
{
    public static Submission Create(
        Guid? id = null,
        Guid? assignmentId = null,
        Guid? studentId = null,
        string? comment = null,
        GradeLetter? grade = null,
        string? feedback = null,
        DateTime? submittedAt = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null
    )
    {
        return new Submission
        {
            Id = id ?? Guid.NewGuid(),

            AssignmentId = assignmentId ?? Guid.NewGuid(),
            StudentId = studentId ?? Guid.NewGuid(),

            Comment = comment,
            Grade = grade,
            Feedback = feedback,

            SubmittedAt = submittedAt ?? DateTime.UtcNow,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = updatedAt
        };
    }
}