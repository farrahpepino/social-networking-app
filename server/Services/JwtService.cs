using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.Services{
    public class JwtService: IJwtService{
        private readonly IConfiguration _configuration;
        private readonly string? _secret;
        private readonly int _expiryMinutes;

        public JwtService (IConfiguration configuration){
            _configuration = configuration;
            _secret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret is missing from config");
            _expiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var minutes) 
                ? minutes 
                : 60; 
        }

        public string GenerateToken(string userId, string username, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Email, email), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}