
using System.ComponentModel.DataAnnotations;

namespace StudyCentral.API.Models.DTOs.Admin.User;

public class AdminUpdateUserPasswordDto
{
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string NewPassword { get; set; } = null!;
}