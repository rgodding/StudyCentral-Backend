using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.ApiModels.AuthModels;

public class SignUpRequestModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    
    [Required]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
    public string LastName { get; set; } = null!;
    
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = null!;
    
}