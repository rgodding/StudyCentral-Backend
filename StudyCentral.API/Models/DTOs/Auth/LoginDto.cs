using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}