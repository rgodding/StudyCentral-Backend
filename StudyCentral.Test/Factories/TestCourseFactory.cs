using StudyCentral.API.Models.Entities;

namespace StudyCentral.Test.Factories;

public static class TestCourseFactory
{
    public static Course Create(
        Guid? id = null,
        string? title = null,
        string? description = null
    )
    {
        return new Course
        {
            Id = id ?? Guid.NewGuid(),
            Title = title ?? $"TITLE_${Guid.NewGuid()}",
            Description = description,
        };
    }
}