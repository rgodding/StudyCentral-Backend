using System.ComponentModel.DataAnnotations;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.Dtos.Auth;

public class SignInDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}