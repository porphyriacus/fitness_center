using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Clients.Commands.Update;
using Application.Features.Clients.Commands.DeleteClient;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ClientsModel : PageModel
    {
        private readonly IMediator _mediator;

        public ClientsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<ClientDto> Clients { get; set; } = new();

        public async Task OnGetAsync(string? searchTerm, string? sortBy, bool sortDescending = false)
        {
            var query = new GetClientsListQuery
            {
                SearchTerm = searchTerm,
                SortBy = sortBy ?? "Id",
                SortDescending = sortDescending
            };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                Clients = result.Value.ToList();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, string name, string surname, string? profilePhotoUrl)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                TempData["EditError"] = "Имя и фамилия обязательны";
                await OnGetAsync(null, null, false);
                return Page();
            }

            var command = new UpdateClientProfileCommand(id, name, surname, profilePhotoUrl);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["EditError"] = result.Error.Message;
                await OnGetAsync(null, null, false);
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _mediator.Send(new DeleteClientCommand(id));
            return RedirectToPage();
        }
    }
}