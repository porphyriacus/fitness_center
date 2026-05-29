using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.Extensions; 

using Application.Features.Clients.Commands.Create;
using Application.Features.Clients.Commands.Update;
using Application.Features.Clients.Commands.DeleteClient;
using Application.Features.Clients.Commands.DeleteClientAvatar;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Clients.Queries.GetClientById;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] string? searchTerm, [FromQuery] string? sortBy, [FromQuery] bool sortDescending = false)
    {
        var query = new GetClientsListQuery { SearchTerm = searchTerm, SortBy = sortBy, SortDescending = sortDescending };
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetClientByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientProfileCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id не совпадает");

        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteClientCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("{id}/avatar")]
    public async Task<IActionResult> DeleteAvatar(int id)
    {
        var command = new DeleteClientAvatarCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}
