using StudyCentral.API.Models.Entities;

namespace StudyCentral.Test.Factories;

public static class TestUserFactory
{
    public static User Create(
        Guid? id = null,
        string? email = null,
        string? hashedPassword = null,
        string? firstName = null,
        string? lastName = null,
        UserRole? role = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null
    )
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Email = email ?? "user@mail.com",
            PasswordHash = hashedPassword ?? "$2a$11$ZZdlueio8rsj67q/d/ZiBe03uM1mX0Y9JfFjwcP/X0KSRiE5G4Ke6",
            FirstName = firstName ?? "John",
            LastName = lastName ?? "Doe",
            Role = role ?? UserRole.Student,
            
        };
    }
}