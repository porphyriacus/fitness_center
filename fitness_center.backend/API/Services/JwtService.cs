using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;


namespace API.Services
{
    /// <summary>
    /// add refresh token later pls
    /// </summary>
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(IdentityUser user)
        {
            // роли пользователя
            var roles = await _userManager.GetRolesAsync(user);

            // claims
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),       // UserId
            new Claim(JwtRegisteredClaimNames.Email, user.Email),  // Email
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // уникальный ID токена
        };

            // роли как отдельные claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // ключ подписи
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // сам токен
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: credentials
            );

            // возвращае строку токена
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
