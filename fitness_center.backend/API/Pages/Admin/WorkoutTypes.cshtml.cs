using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Application.Features.WorkoutTypes;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using Application.Features.WorkoutTypes.Commands.Create;
using Application.Features.WorkoutTypes.Commands.Update;
using Application.Features.WorkoutTypes.Commands.Delete;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class WorkoutTypesModel : PageModel
    {
        private readonly IMediator _mediator;

        public WorkoutTypesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<WorkoutTypeDto> WorkoutTypes { get; set; } = new();

        public async Task OnGetAsync(string? searchField, string? searchTerm)
        {
            var query = new GetWorkoutTypesListQuery
            {
                SearchField = searchField?.ToLower(),
                SearchTerm = searchTerm?.ToLower()
            };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                WorkoutTypes = result.Value.ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, string description, int defaultDurationMinutes, int defaultMaxCapacity, decimal price, string? color, string? colorText)
        {
            var finalColor = color ?? colorText;

            var command = new CreateWorkoutTypeCommand(name, description ?? "", defaultDurationMinutes, defaultMaxCapacity, finalColor, price);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["CreateError"] = result.Error.Message;
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, string description, string? color, string? colorText, decimal price)
        {
            var finalColor = color ?? colorText;

            var command = new UpdateWorkoutTypeCommand(id, description ?? "", finalColor, price);
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
            await _mediator.Send(new DeleteWorkoutTypeCommand(id));
            return RedirectToPage();
        }
    }
}