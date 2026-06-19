namespace StudyCentral.API.Models.Entities.Chat;

public class ChatRoomMember
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastSeenAt { get; set; }
}