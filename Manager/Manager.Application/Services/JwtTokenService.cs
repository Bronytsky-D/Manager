using Manager.Application.Astraction.Services;
using Manager.Domain.Entites;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Manager.Application.Services
{
    public class JwtTokenService : IJwtTokenService  
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
             {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                 new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? "")
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSetting:SecurityKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWTSetting:ValidIssuer"],
                audience: _config["JWTSetting:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
