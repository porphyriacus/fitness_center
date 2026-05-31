using API.Extensions;
using Application.Features.MembershipTypes.Commands.Create;
using Application.Features.MembershipTypes.Commands.Delete;
using Application.Features.MembershipTypes.Commands.Update;
using Application.Features.MembershipTypes.Queries.GetMembershipTypeById;
using Application.Features.MembershipTypes.Queries.GetMembershipTypesList;
using Application.Features.WorkoutTypes.Commands.Create;
using Application.Features.WorkoutTypes.Commands.Delete;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypeById;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MembershipTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MembershipTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> GetList([FromQuery] string? searchTerm, [FromQuery] string? sortBy, [FromQuery] bool sortDescending = false)
        {
            var query = new GetMembershipTypesListQuery
            {
                SearchTerm = searchTerm,
                SortBy = sortBy,
                SortDescending = sortDescending
            };
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetMembershipTypeByIdQuery(id);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        /// 

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMembershipTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMembershipTypeCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id не совпадает");

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteMembershipTypeCommand(id);
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
