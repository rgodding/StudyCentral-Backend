using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserDto = StudyCentral.API.Models.DTOs.User.UserDto;

namespace StudyCentral.API.Authentication;

public interface IJwtService
{
    string GenerateToken(UserDto userDto);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(UserDto userDto)
    {
        var jwtKey = _configuration["JWT:Key"]
                     ?? throw new InvalidOperationException("JWT:Key not configured");

        var issuer = _configuration["JWT:Issuer"]
                     ?? throw new InvalidOperationException("JWT:Issuer not configured");

        var audience = _configuration["JWT:Audience"]
                       ?? throw new InvalidOperationException("JWT:Audience not configured");

        var expirationDays = int.Parse(
            _configuration["JWT:ExpirationDays"]
            ?? throw new InvalidOperationException("JWT:ExpirationDays not configured"));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
            new(ClaimTypes.Email, userDto.Email),
            new(ClaimTypes.Role, userDto.Role.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(expirationDays),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}