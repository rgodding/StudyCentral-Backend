

using StudyCentral.API.Models.Entities.Enums;

namespace StudyCentral.API.Models.Entities.Chat;

public class ChatRoom
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ChatRoomType Type { get; set; }

    public Guid? CourseId { get; set; }
    public Course? Course { get; set; }

    public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}