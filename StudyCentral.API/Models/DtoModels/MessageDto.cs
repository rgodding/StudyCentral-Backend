namespace StudyCentral.API.Models.DtoModels;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentAt { get; set; }
    
    public UserDto User { get; set; } = null!;
    public ChatDto Chat { get; set; } = null!;
}
