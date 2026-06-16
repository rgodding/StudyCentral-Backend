using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.DTOs.Chat.ChatRoom;


public class ChatRoomDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ChatRoomType Type { get; set; }

    public Guid? CourseId { get; set; }

    public string? CourseName { get; set; }

    public int MemberCount { get; set; }

    public string? LastMessagePreview { get; set; }

    public DateTime? LastMessageAt { get; set; }

    public DateTime CreatedAt { get; set; }
}