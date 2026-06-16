using StudyCentral.API.Models.Entities.Chat;

namespace StudyCentral.Test.Factories;

public static class TestChatMessageFactory
{
    public static ChatMessage Create(
        Guid? id = null,
        Guid? chatRoomId = null,
        Guid? senderId = null,
        string? content = null,
        DateTime? createdAt = null,
        DateTime? editedAt = null,
        DateTime? deletedAt = null
    )
    {
        return new ChatMessage
        {
            Id = id ?? Guid.NewGuid(),
            ChatRoomId = chatRoomId ?? Guid.NewGuid(),
            SenderId = senderId ?? Guid.NewGuid(),
            Content = content ?? $"CONTENT_{Guid.NewGuid()}",
            CreatedAt = createdAt ?? DateTime.UtcNow,
            EditedAt = editedAt,
            DeletedAt = deletedAt
        };
    }
}