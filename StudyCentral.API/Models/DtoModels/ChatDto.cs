using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DtoModels;

public class ChatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ChatType Type { get; set; }
    
    public CourseDto? Course { get; set; }
    
    public ICollection<MessageDto> Messages { get; set; } = new List<MessageDto>();
    public ICollection<UserDto> Participants { get; set; } = new List<UserDto>();
}