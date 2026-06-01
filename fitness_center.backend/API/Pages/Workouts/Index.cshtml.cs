using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Application.Features.Workouts;
using Application.Features.Workouts.Queries.GetWorkoutsList;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using Application.Features.WorkoutTypes;

namespace API.Pages.Workouts
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<WorkoutDto> Workouts { get; set; } = new();
        public List<WorkoutTypeDto> WorkoutTypes { get; set; } = new();

        public async Task OnGetAsync()
        {
            var typesResult = await _mediator.Send(new GetWorkoutTypesListQuery());
            if (typesResult.IsSuccess)
                WorkoutTypes = typesResult.Value.ToList();

            var query = new GetWorkoutsListQuery
            {
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(14),
                IncludePast = false
            };

            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                Workouts = result.Value.ToList();
        }
    }
}