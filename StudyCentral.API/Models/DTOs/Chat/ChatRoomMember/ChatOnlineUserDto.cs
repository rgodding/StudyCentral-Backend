namespace StudyCentral.API.Models.DTOs.Chat.ChatRoomMember;

public class ChatOnlineUserDto
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
}