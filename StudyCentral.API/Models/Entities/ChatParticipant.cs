namespace StudyCentral.Data.Entities;

public class ChatParticipant
{
    public Guid ChatRoomId { get; set; }
    public Chat ChatRoom { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}