namespace StudyCentral.API.Models.DTOs.Chat.ChatMessage;

public class ChatMessageDto
{
    public Guid Id { get; set; }

    public Guid ChatRoomId { get; set; }

    public Guid SenderId { get; set; }

    public string SenderName { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? EditedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}