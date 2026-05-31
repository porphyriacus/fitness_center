using API.Extensions;
using Application.Features.Bookings.Commands.CancelBooking;
using Application.Features.Bookings.Queries;
using Application.Features.Bookings.Queries.GetClientBookingsHistory;
using Application.Features.Clients.Commands.Create;
using Application.Features.Clients.Commands.DeleteClient;
using Application.Features.Clients.Commands.DeleteClientAvatar;
using Application.Features.Clients.Commands.Update;
using Application.Features.Clients.Queries.GetClientById;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Memberships.Queries.GetClientMembership;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthorizationService _authorizationService;
    public ClientsController(IMediator mediator, IAuthorizationService authorizationService)
    {
        _mediator = mediator;
        _authorizationService = authorizationService;
    }

    /// ADMIN
    /*
        GET api/clients
            api/clients/id
        PUT
            api/clients/id
        DELETE
            api/clients/id
    */
    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchTerm">поиск по имени и фамилии   "name", "surname", "registeredAt"</param>
    /// <param name="sortBy">по какому полю сортировать(по id)  "Name", "Surname", "RegisteredAt"</param>
    /// <param name="sortDescending"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetList([FromQuery] string? searchTerm, [FromQuery] string? sortBy, [FromQuery] bool sortDescending = false)
    {
        var query = new GetClientsListQuery { SearchTerm = searchTerm, SortBy = sortBy, SortDescending = sortDescending };
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetClientByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientProfileCommand command)
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
        var command = new DeleteClientCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }


    /// CLIENT
    /*
        GET 
            api/clients/profile
            api/clients/memberships
            api/clients/bookings
            api/clients/bookings/history
        POST
            api/clients/profile
        PUT
            api/clients/profile
            api/clients/bookings/cancel
        DELETE
            api/clients/profile/avatar
    */

    [HttpGet("profile")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetProfile([FromQuery] int id)
    {
        // 1. Проверяем права, используя id из тела запроса
        var authResult = await _authorizationService.AuthorizeAsync(
            User,                    // кто (пользователь из токена)
            id,              // что (id из тела запроса)
            "ClientOwnerPolicy");    // по какой политике

        if (!authResult.Succeeded)
        {
            return Forbid();
        }
        var command = new GetClientByIdQuery(id);
        var result = await _mediator.Send(command); 
        return result.ToActionResult();
    }

    [HttpGet("memberships")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetClientsMembership([FromQuery] int id)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User
            , id
            , "ClientOwnerPolicy"
            );
        if(!authResult.Succeeded)
            return Forbid();
        var command = new GetClientMembershipQuery(id);
        var result = await _mediator.Send(command); 
        return result.ToActionResult();
    }
    [HttpGet("bookings")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetClientsBookings([FromQuery] int id)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User
            , id
            , "ClientOwnerPolicy"
            );
        if (!authResult.Succeeded)
            return Forbid();
        var command = new GetClientBookingsQuery(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
    [HttpGet("bookings/history")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetClientsBookingsHistory([FromQuery] int id)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User
            , id
            , "ClientOwnerPolicy"
            );
        if (!authResult.Succeeded)
            return Forbid();
        var command = new GetClientBookingsHistoryQuery(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }


    [HttpPost("profile")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateClientCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateClientProfileCommand command)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User
            , command.Id
            , "ClientOwnerPolicy"
            );
        if (!authResult.Succeeded)
            return Forbid();

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("bookings/cancel")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CancelBooking([FromBody] CancelBookingCommand command)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User
            , command.BookingId
            , "BookingOwnerPolicy"
            );
        if (!authResult.Succeeded)
            return Forbid();

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("avatar")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> DeleteAvatar([FromQuery] int id)
    {
        var authResult = await _authorizationService.AuthorizeAsync(
            User
            , id
            , "ClientOwnerPolicy"
        );
        if (!authResult.Succeeded)
            return Forbid();
        var command = new DeleteClientAvatarCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }




}
