using API.DTOs.Auth;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{


    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        public AuthController(UserManager<IdentityUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(new { error = errors });
            }

            // роль "Client" по умолчанию
            await _userManager.AddToRoleAsync(user, "Client");

            return Ok(new { UserId = user.Id, Email = user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { error = "Неверный email или пароль" });
            }

            // проверяет пароль без создания кукав
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { error = "Неверный email или пароль" });
            }

            // генерируем JWT токен
            var token = await _jwtService.GenerateTokenAsync(user);

            return Ok(new AuthResponse
            {
                AccessToken = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                UserId = user.Id,
                Email = user.Email
            });
        }
    }
}
