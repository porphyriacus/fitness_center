using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Application.Features.MembershipTypes;
using Application.Features.MembershipTypes.Queries.GetMembershipTypesList;
using Application.Features.MembershipTypes.Commands.Create;
using Application.Features.MembershipTypes.Commands.Update;
using Application.Features.MembershipTypes.Commands.Delete;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class MembershipTypesModel : PageModel
    {
        private readonly IMediator _mediator;

        public MembershipTypesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<MembershipTypeDto> MembershipTypes { get; set; } = new();

        public async Task OnGetAsync(string? searchTerm)
        {
            var query = new GetMembershipTypesListQuery
            {
                SearchTerm = searchTerm,
                SortBy = "Id",
                SortDescending = false
            };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                MembershipTypes = result.Value.ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync(
            string name,
            string? description,
            decimal price,
            int? sessionsCount,
            int validityDays,
            bool canFreeze,
            int? maxFreezeDays)
        {
            // Если CanFreeze false, maxFreezeDays должен быть null
            if (!canFreeze)
                maxFreezeDays = null;

            // Если SessionsCount не указан или 0, делаем null (безлимит)
            if (sessionsCount.HasValue && sessionsCount.Value <= 0)
                sessionsCount = null;

            var command = new CreateMembershipTypeCommand(name, description, price, sessionsCount, validityDays, canFreeze, maxFreezeDays);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["CreateError"] = result.Error.Message;
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, string name, string? description, decimal price)
        {
            var command = new UpdateMembershipTypeCommand(id, name, description, price);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["EditError"] = result.Error.Message;
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _mediator.Send(new DeleteMembershipTypeCommand(id));
            return RedirectToPage();
        }
    }
}