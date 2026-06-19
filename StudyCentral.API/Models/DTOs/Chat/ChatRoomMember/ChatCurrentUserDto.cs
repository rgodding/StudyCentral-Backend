namespace StudyCentral.API.Models.DTOs.Chat.ChatRoomMember;

public class ChatCurrentUserDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }
}