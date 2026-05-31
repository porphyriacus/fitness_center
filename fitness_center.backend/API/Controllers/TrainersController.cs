using API.Extensions;
using Application.Features.Clients.Queries.GetClientById;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Trainers.Commands.Create;
using Application.Features.Trainers.Commands.Delete;
using Application.Features.Trainers.Commands.Update;
using Application.Features.Trainers.Queries.GetTrainerById;
using Application.Features.Trainers.Queries.GetTrainerSchedule;
using Application.Features.Trainers.Queries.GetTrainersList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;
        public TrainersController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        /// ADMIN
        /*
            GET api/trainers
                api/trainers/id
            PUT
                api/trainers/id
            POST
                api/trainers
            DELETE
                api/trainers/id
        */
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTrainerByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetList([FromQuery] string? searchTerm, [FromQuery] string? sortBy, [FromQuery] bool sortDescending = false)
        {
            var query = new GetTrainersListQuery { SearchTerm = searchTerm, SortBy = sortBy, SortDescending = sortDescending };
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTrainerCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id не совпадает");

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateTrainerCommand command)
        {

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteTrainerCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// TRAINER
        /*
            GET 
                api/trainers/profile
                api/trainers/schedule
            PUT
                api/trainers/profile
        */
        [HttpGet("profile")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetProfile([FromQuery] int id)
        {
            var authResult = await _authorizationService.AuthorizeAsync(
                User
                , id
                , "TrainerOwnerPolicy");
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            var query = new GetTrainerByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("schedule")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetSchedule(
                                                    [FromQuery] int id,
                                                    [FromQuery] DateTime? fromDate = null,
                                                    [FromQuery] DateTime? toDate = null,
                                                    [FromQuery] bool includePast = false)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, id, "TrainerOwnerPolicy");
            if (!authResult.Succeeded)
                return Forbid();

            var query = new GetTrainerScheduleQuery
            {
                TrainerId = id,
                FromDate = fromDate,
                ToDate = toDate,
                IncludePast = includePast
            };

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpPut("profile")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> Update([FromBody] UpdateTrainerCommand command)
        {

            var authResult = await _authorizationService.AuthorizeAsync(User, command.Id, "TrainerOwnerPolicy");
            if (!authResult.Succeeded)
                return Forbid();

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

    }
}
