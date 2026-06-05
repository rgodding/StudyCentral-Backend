using StudyCentral.API.Models.Entities;

namespace StudyCentral.Test.Factories;

public static class TestAnnouncementFactory
{
    public static Announcement Create(
        Guid? id = null,
        Guid? courseId = null,
        string? name = null,
        string? content = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null
    )
    {
        return new Announcement
        {
            Id = id ?? Guid.NewGuid(),

            CourseId = courseId ?? Guid.NewGuid(),

            Name = name ?? $"ANNOUNCEMENT_{Guid.NewGuid()}",
            Content = content ?? "Test announcement content",

            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = updatedAt
        };
    }
}