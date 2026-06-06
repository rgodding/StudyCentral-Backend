using StudyCentral.API.Models.ApiModels.Auth;

namespace StudyCentral.API.Constants.Tests;

public static class TestLoginRequests
{
    public static readonly LoginRequest Admin = new()
    {
        Email = "admin@studycentral.dk",
        Password = "password"
    };

    public static readonly LoginRequest Teacher = new()
    {
        Email = "teacher@studycentral.dk",
        Password = "password"
    };

    public static readonly LoginRequest Student = new()
    {
        Email = "student@studycentral.dk",
        Password = "password"
    };
}