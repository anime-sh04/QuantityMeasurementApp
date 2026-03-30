using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementAppModelLayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuantityMeasurementApp.Api.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(ApplicationUser user);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config) => _config = config;

        public string GenerateToken(ApplicationUser user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key        = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds      = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role,               user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName",  user.LastName  ?? string.Empty),
            };

            int expiresMinutes = int.TryParse(jwtSection["ExpiresMinutes"], out var m) ? m : 60;

            var token = new JwtSecurityToken(
                issuer:             jwtSection["Issuer"],
                audience:           jwtSection["Audience"],
                claims:             claims,
                notBefore:          DateTime.UtcNow,
                expires:            DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
