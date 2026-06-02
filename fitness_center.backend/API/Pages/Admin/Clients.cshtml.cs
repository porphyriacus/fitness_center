using Application.Features.Clients.Commands.DeleteClient;
using Application.Features.Clients.Commands.Update;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Memberships.Commands.AssignMembershipToClient;
using Application.Features.Memberships.Commands.FreezeMembership;
using Application.Features.Memberships.Commands.UnfreezeMembership;
using Application.Features.Memberships.Queries.GetClientMembership;
using Application.Features.MembershipTypes;
using Application.Features.MembershipTypes.Queries.GetMembershipTypesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public List<MembershipTypeDto> MembershipTypes { get; set; } = new();
        public List<ClientDto> Clients { get; set; } = new();

        public async Task OnGetAsync(string? searchTerm, string? sortBy, bool sortDescending = false)
        {
            // ≈сли есть поисковый запрос, ищем сначала в нижнем регистре
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var queryLower = new GetClientsListQuery
                {
                    SearchTerm = searchTerm.ToLower(),
                    SortBy = sortBy ?? "Id",
                    SortDescending = sortDescending
                };
                var resultLower = await _mediator.Send(queryLower);

                if (resultLower.IsSuccess && resultLower.Value.Any())
                {
                    Clients = resultLower.Value.ToList();
                }
                else
                {
                    // ≈сли не нашли, пробуем с заглавной первой буквой
                    var capitalized = char.ToUpper(searchTerm[0]) + searchTerm[1..].ToLower();
                    var queryCapitalized = new GetClientsListQuery
                    {
                        SearchTerm = capitalized,
                        SortBy = sortBy ?? "Id",
                        SortDescending = sortDescending
                    };
                    var resultCapitalized = await _mediator.Send(queryCapitalized);

                    if (resultCapitalized.IsSuccess)
                    {
                        Clients = resultCapitalized.Value.ToList();
                    }
                }
            }
            else
            {
                // Ѕез поиска Ц просто загружаем всех
                var query = new GetClientsListQuery
                {
                    SortBy = sortBy ?? "Id",
                    SortDescending = sortDescending
                };
                var result = await _mediator.Send(query);
                if (result.IsSuccess)
                {
                    Clients = result.Value.ToList();
                }
            }

            // «агружаем типы абонементов дл€ модалки назначени€
            var typesResult = await _mediator.Send(new GetMembershipTypesListQuery());
            if (typesResult.IsSuccess)
            {
                MembershipTypes = typesResult.Value.ToList();
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, string name, string surname, string? profilePhotoUrl)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                TempData["EditError"] = "»м€ и фамили€ об€зательны";
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

        public async Task<IActionResult> OnGetMembership(int clientId)
        {
            var query = new GetClientMembershipQuery(clientId);
            var result = await _mediator.Send(query);
            return new JsonResult(result.IsSuccess ? result.Value : null);
        }

        public async Task<IActionResult> OnPostAssignMembership(int clientId, int membershipTypeId)
        {
            var command = new AssignMembershipToClientCommand(clientId, membershipTypeId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["MembershipError"] = result.Error.Message;
                return RedirectToPage();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostFreezeMembership(int clientId, int days)
        {
            var command = new FreezeMembershipCommand(clientId, TimeSpan.FromDays(days));
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["MembershipError"] = result.Error.Message;
                return RedirectToPage();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnfreezeMembership(int clientId)
        {
            var command = new UnfreezeMembershipCommand(clientId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["MembershipError"] = result.Error.Message;
                return RedirectToPage();
            }

            return RedirectToPage();
        }
    }
}