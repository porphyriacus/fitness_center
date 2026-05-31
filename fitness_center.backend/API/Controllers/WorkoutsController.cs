using API.Extensions;
using Application.Features.Bookings.Commands.Create;
using Application.Features.Bookings.Queries.GetBookingsByWorkout;
using Application.Features.Workouts.Commands.Cancel;
using Application.Features.Workouts.Commands.Close;
using Application.Features.Workouts.Commands.Create;
using Application.Features.Workouts.Commands.Delete;
using Application.Features.Workouts.Commands.Update;
using Application.Features.Workouts.Queries.GetWorkoutById;
using Application.Features.Workouts.Queries.GetWorkoutsList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class WorkoutsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;
        public WorkoutsController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }


        [HttpGet]
        public async Task<IActionResult> GetList(
            [FromQuery] int? workoutTypeId,
            [FromQuery] int? trainerId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] bool includePast = true,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDescending = false)
        {
            var query = new GetWorkoutsListQuery
            {
                WorkoutTypeId = workoutTypeId,
                TrainerId = trainerId,
                FromDate = fromDate,
                ToDate = toDate,
                IncludePast = includePast,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortDescending = sortDescending
            };
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetWorkoutByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        /// 

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkoutCommand command)
        {
            if (id != command.WorkoutId)
                return BadRequest("Id в маршруте и в теле не совпадают");

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteWorkoutCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("cancel/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int id)
        {
            var command = new CancelWorkoutCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPatch("close/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Close(int id)
        {
            var command = new CloseWorkoutCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// забронировать тренировку
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/bookings")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateBooking(int id, [FromBody] CreateBookingCommand command)
        {
            if (id != command.WorkoutId)
                return BadRequest("Id тренировки в маршруте и в теле не совпадают");

            var authResult = await _authorizationService.AuthorizeAsync(
                User
                , command.ClientId
                , "ClientOwnerPolicy"
            );
            if (!authResult.Succeeded)
                return Forbid();

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// список клиентов записанных на тренировку (админ/тренер)
        /// </summary>
        [HttpGet("{id}/bookings")]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> GetBookingsByWorkout(int id)
        {
            var query = new GetBookingsByWorkoutQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }
    }
}
