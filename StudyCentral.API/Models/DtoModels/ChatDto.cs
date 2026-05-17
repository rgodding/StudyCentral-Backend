using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DtoModels;

public class ChatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ChatType Type { get; set; }
}