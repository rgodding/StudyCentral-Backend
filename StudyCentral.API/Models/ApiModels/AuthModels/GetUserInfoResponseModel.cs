namespace StudyCentral.API.Models.ApiModels.AuthModels;

public class GetUserInfoResponseModel
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}