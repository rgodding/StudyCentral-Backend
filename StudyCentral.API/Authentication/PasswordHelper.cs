namespace StudyCentral.API.Authentication;

public static class PasswordHelper
{
    public static string HashPassword(string message)
    {
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(message);
        return hashPassword;
    }

    public static bool VerifyHash(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
}