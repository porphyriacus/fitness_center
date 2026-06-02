using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Application.Features.Specializations;
using Application.Features.Specializations.Queries.GetSpecializationsList;
using Application.Features.Specializations.Commands.Create;
using Application.Features.Specializations.Commands.DeleteSpecialization;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class SpecializationsModel : PageModel
    {
        private readonly IMediator _mediator;

        public SpecializationsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<SpecializationDto> Specializations { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await _mediator.Send(new GetSpecializationsQuery());
            if (result.IsSuccess)
                Specializations = result.Value.ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                TempData["CreateError"] = "Название должно содержать минимум 2 символа";
                await LoadData();
                return Page();
            }

            if (name.Length > 100)
            {
                TempData["CreateError"] = "Название не может превышать 100 символов";
                await LoadData();
                return Page();
            }

            var command = new CreateSpecializationCommand(name.Trim());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["CreateError"] = result.Error.Message;
                await LoadData();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var command = new DeleteSpecializationCommand(id);
            await _mediator.Send(command);
            return RedirectToPage();
        }
    }
}