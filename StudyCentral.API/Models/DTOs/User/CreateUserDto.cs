using System.ComponentModel.DataAnnotations;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.User;

public class CreateUserDto
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;
    
    [Required]
    [MinLength(6)]
    [MaxLength(255)]
    public string Password { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
}