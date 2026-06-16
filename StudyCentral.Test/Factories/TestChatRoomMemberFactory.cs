using StudyCentral.API.Models.Entities.Chat;
using StudyCentral.API.Models.Entities.Relationship;

namespace StudyCentral.Test.Factories;

public static class TestChatRoomMemberFactory
{
    public static ChatRoomMember Create(
        Guid? userId = null,
        Guid? chatRoomId = null,
        DateTime? joinedAt = null
    )
    {
        return new ChatRoomMember
        {
            UserId = userId ?? Guid.NewGuid(),
            ChatRoomId = chatRoomId ?? Guid.NewGuid(),
            JoinedAt = joinedAt ?? DateTime.UtcNow
        };
    }
}