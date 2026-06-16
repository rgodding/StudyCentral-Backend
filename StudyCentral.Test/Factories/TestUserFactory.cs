using StudyCentral.API.Models.Entities;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.Test.Factories;

public static class TestUserFactory
{
    public static User Create(
        Guid? id = null,
        string? email = null,
        string? passwordHash = null,
        string? firstName = null,
        string? lastName = null,
        UserRole? role = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null,
        List<Course>? teachingCourses = null,
        StudyFile? profilePicture = null
    )
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Email = email ?? $"user_{Guid.NewGuid()}@mail.com",
            PasswordHash = passwordHash ?? "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
            FirstName = firstName ?? $"USER_{Guid.NewGuid()}",
            LastName = lastName ?? $"USER_{Guid.NewGuid()}",
            Role = role ?? UserRole.Student,

            TeachingCourses = teachingCourses ?? new List<Course>(),

            ProfilePicture = profilePicture,

            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = updatedAt
        };
    }
}