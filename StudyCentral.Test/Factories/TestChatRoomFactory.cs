using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.Test.Factories;

public static class TestChatRoomFactory
{
    public static ChatRoom Create(
        Guid? id = null,
        string? name = null,
        ChatRoomType? chatRoomType = null,
        Guid? courseId = null,
        DateTime? createdAt = null,
        DateTime? updatedAt = null
    )
    {
        return new ChatRoom
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? $"NAME_{Guid.NewGuid()}",
            Type = chatRoomType ?? ChatRoomType.Course,
            CourseId = courseId,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = updatedAt
        };
    }
}