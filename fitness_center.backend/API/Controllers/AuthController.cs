using API.DTOs.Auth;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace API.Controllers
{


    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly RoleManager<IdentityRole> _roleManager;  // 
        public AuthController(UserManager<IdentityUser> userManager, JwtService jwtService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
        }


        [HttpGet("my-roles")]
        [Authorize]
        public async Task<IActionResult> GetMyRoles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { UserId = userId, Roles = roles });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(new { error = errors });
            }


            var roleExists = await _roleManager.RoleExistsAsync("Client");
            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole("Client"));

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

        [HttpPost("register-trainer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterTrainer([FromBody] RegisterRequest request)
        {

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(new { error = "Пользователь с таким email уже существует" });


            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return BadRequest(new { error = errors });
            }


            if (!await _roleManager.RoleExistsAsync("Trainer"))
                await _roleManager.CreateAsync(new IdentityRole("Trainer"));

            await _userManager.AddToRoleAsync(user, "Trainer");

            return Ok(new { UserId = user.Id, Email = user.Email });
        }
    }
}
