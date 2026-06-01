using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace API.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Введите email")]
            [EmailAddress(ErrorMessage = "Некорректный email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Запомнить меня")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    Input.Password,
                    false,  // ?? Ставим false, потом сами создадим куку
                    lockoutOnFailure: true
                );

                if (result.Succeeded)
                {
                    // ?? ПРИНУДИТЕЛЬНО создаём куку с ролями
                    await CreateAuthCookieWithRoles(user);
                    var rolesAfterLogin = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                    var rolesList = string.Join(", ", rolesAfterLogin);
                    Console.WriteLine($"Роли в куке: {rolesList}");

                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Аккаунт заблокирован. Попробуйте позже.");
                    return Page();
                }
                ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
            }
            return Page();
        
    }

      
        private async Task CreateAuthCookieWithRoles(IdentityUser user)
        {
            // 1. Получаем роли пользователя из Identity
            var roles = await _userManager.GetRolesAsync(user);

            // 2. Создаём список claims (утверждений)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
    };

            // 3. Добавляем каждую роль как отдельный claim
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 4. Создаем identity и principal
            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // 5. Настройки куки (время жизни)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,  // "Запомнить меня"
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(Input.RememberMe ? 30 : 1)
            };

            // 6. Выходим из текущей сессии (удаляем старую куку)
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // 7. Создаём новую куку с ролями
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal, authProperties);
        }
    }
}