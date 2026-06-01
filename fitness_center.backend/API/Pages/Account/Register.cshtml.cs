using Application.Features.Clients.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace API.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMediator _mediator;

        public RegisterModel(UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager,
                             IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Введите email")]
            [EmailAddress(ErrorMessage = "Некорректный email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Введите имя")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Введите фамилию")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Введите пароль")]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "Пароль должен содержать минимум 6 символов", MinimumLength = 6)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var identityUser = new IdentityUser { UserName = Input.Email, Email = Input.Email };
            var identityResult = await _userManager.CreateAsync(identityUser, Input.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _userManager.AddToRoleAsync(identityUser, "Client");

            var createClientCommand = new CreateClientCommand(Input.Name, Input.Surname, identityUser.Id, null);
            var result = await _mediator.Send(createClientCommand);

            if (!result.IsSuccess)
            {
                await _userManager.DeleteAsync(identityUser);
                ModelState.AddModelError(string.Empty, "Ошибка при создании профиля клиента");
                return Page();
            }

            await _signInManager.SignInAsync(identityUser, isPersistent: false);
            return RedirectToPage("/Index");
        }
    }
}