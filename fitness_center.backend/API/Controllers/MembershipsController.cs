using API.Extensions;
using Application.Features.Bookings.Commands.Create;
using Application.Features.Bookings.Queries.GetBookingsByWorkout;
using Application.Features.Memberships.Commands.AssignMembershipToClient;
using Application.Features.Memberships.Commands.FreezeMembership;
using Application.Features.Memberships.Commands.UnfreezeMembership;
using Application.Features.Memberships.Queries.GetClientMembership;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MembershipsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public MembershipsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// назначить абонемент клиенту
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignMembershipToClientCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// заморозить абонемент клиента
        /// </summary>
        [HttpPost("freeze")]
        public async Task<IActionResult> Freeze([FromBody] FreezeMembershipCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// разморозить абонемент клиента
        /// </summary>
        [HttpPost("unfreeze")]
        public async Task<IActionResult> Unfreeze([FromBody] UnfreezeMembershipCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// получить абонемент клиента (для админа)
        /// </summary>
        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var query = new GetClientMembershipQuery(clientId);
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }
    }
}
