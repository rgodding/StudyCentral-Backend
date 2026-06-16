namespace StudyCentral.API.Models.Entities.Chat;

public class ChatMessage
{
    public Guid Id { get; set; }

    public Guid ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}