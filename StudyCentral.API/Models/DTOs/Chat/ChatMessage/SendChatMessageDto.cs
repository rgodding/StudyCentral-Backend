using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Chat.ChatMessage;

public class SendChatMessageDto
{
    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = null!;
}