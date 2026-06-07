using StudyCentral.API.Models.DTOs.Auth;

namespace StudyCentral.API.Constants.Tests;

public static class TestLoginDto
{
    public static readonly LoginDto Admin = new()
    {
        Email = "admin@studycentral.dk",
        Password = "password"
    };

    public static readonly LoginDto Teacher = new()
    {
        Email = "teacher@studycentral.dk",
        Password = "password"
    };

    public static readonly LoginDto Student = new()
    {
        Email = "student@studycentral.dk",
        Password = "password"
    };
}