namespace StudyCentral.API.Models.Entities;

public class Message
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;
    
    public DateTime SentAt { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
}