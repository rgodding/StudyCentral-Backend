using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Models.DTOs.User;

public class ChangeUserRoleDto
{
    public UserRole Role { get; set; }
}