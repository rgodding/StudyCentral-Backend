namespace StudyCentral.API.Models.DTOs.User;

public class UserPreviewDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }
}