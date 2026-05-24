using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.Dtos.Auth;

public class SignUpDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;
}