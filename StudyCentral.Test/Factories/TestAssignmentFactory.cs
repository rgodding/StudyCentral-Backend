using StudyCentral.API.Models.Entities;

namespace StudyCentral.Test.Factories;

public static class TestAssignmentFactory
{
    public static Assignment Create(
        Guid? id = null,
        string? title = null,
        string? description = null,
        Guid? courseId = null,
        DateTime? deadline = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null
    )
    {
        return new Assignment
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? $"TITLE_{Guid.NewGuid()}",
            Description = description,
            CourseId = courseId ?? Guid.NewGuid(),
            Deadline  = deadline ?? DateTime.UtcNow.AddDays(7),
            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = updatedAt
        };
    }
}