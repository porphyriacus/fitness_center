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
using Application.Common.Models;
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

        public PagedResult<ClientDto>? Clients { get; set; }
        public List<MembershipTypeDto> MembershipTypes { get; set; } = new();

        public async Task OnGetAsync(
            int pageNumber = 1, 
            int pageSize = 10, 
            string? searchTerm = null, 
            string? sortBy = null, 
            bool sortDescending = false)
        {
            ViewData["SearchTerm"] = searchTerm ?? "";
            ViewData["PageSize"] = pageSize.ToString();
            ViewData["SortBy"] = sortBy ?? "Id";
            ViewData["SortDescending"] = sortDescending.ToString();

            var query = new GetClientsListQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                SortBy = sortBy ?? "Id",
                SortDescending = sortDescending
            };

            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                Clients = result.Value;
            }

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
                TempData["EditError"] = "Имя и фамилия обязательны";
                return RedirectToPage();
            }

            var command = new UpdateClientProfileCommand(id, name, surname, profilePhotoUrl);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["EditError"] = result.Error.Message;
                return RedirectToPage();
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