using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Fitness.Utils;

public class JwtUtil(IConfiguration config)
{
    public string GenerateJwtToken(User.User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var jwtSettings = config.GetSection("JwtSettings");
        
        // Проверка всех необходимых значений
        var secretKey = jwtSettings["SecretKey"] ?? 
                        throw new ArgumentException("JWT SecretKey is not configured");
        
        if (!double.TryParse(jwtSettings["ExpiryInHours"], out var expiryInHours))
            expiryInHours = 24; // Значение по умолчанию

        var issuer = jwtSettings["Issuer"] ?? "default_issuer";
        var audience = jwtSettings["Audience"] ?? "default_audience";

        var key = Encoding.ASCII.GetBytes(secretKey);

        // Создаем claims безопасно
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(expiryInHours),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to generate JWT token", ex);
        }
    }
}