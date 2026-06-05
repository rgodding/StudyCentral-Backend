using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.User;

public class UpdateUserDto
{
    [EmailAddress]
    [MaxLength(255)]
    [MinLength(5)]
    public string? Email { get; set; }

    [MaxLength(50)]
    [MinLength(2)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    [MinLength(2)]
    public string? LastName { get; set; }
}