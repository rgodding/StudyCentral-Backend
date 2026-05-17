namespace StudyCentral.API.Models.Entities;

public class Chat
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public ChatType Type { get; set; }
    
    public Guid? CourseId { get; set; }
    public Course? Course { get; set; } = null!;
    
    public List<Message> Messages { get; set; } = new List<Message>();
    public ICollection<User> Participants { get; set; } = new List<User>();
}

public enum ChatType
{
    Private,
    Group,
    Course
}