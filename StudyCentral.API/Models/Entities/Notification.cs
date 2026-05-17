namespace StudyCentral.Data.Entities;

public class Notification
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public bool IsRead { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}