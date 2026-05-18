using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using StudyCentral.API.Models.Entities;

namespace StudyCentral.API.Authentication;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    /// <summary>
    /// Get a user based on the given JWT Token
    /// </summary>
    public static UserPrincipal GetUser(ClaimsPrincipal claims)
    {
        return new UserPrincipal
        {
            Id = Guid.Parse(
                claims.FindFirst(ClaimTypes.NameIdentifier)!.Value
            ),
            Role = claims.FindFirst(ClaimTypes.Role)!.Value,
        };
    }
    
    /// <summary>
    /// Generates a token based on the user
    /// </summary>
    public string GenerateToken(User user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        
        var jwtKey = _configuration["JWT:Key"];
        if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key not set");
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddDays(30),
            Issuer = "StudyCentral.dk",
            Audience = "StudyCentral.dk",
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(authClaims)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
}

public class UserPrincipal
{
    public Guid Id { get; set; }
    public string Role { get; set; }
}