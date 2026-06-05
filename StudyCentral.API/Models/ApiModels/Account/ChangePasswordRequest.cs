namespace StudyCentral.API.Models.ApiModels.Account;

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}