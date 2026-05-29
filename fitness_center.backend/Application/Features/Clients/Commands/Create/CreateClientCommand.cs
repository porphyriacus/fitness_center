using Application.Common.Models;
using Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.Create
{
    public sealed record CreateClientCommand(string Name, string Surname, string IdentityUserId, string? ProfilePhotoUrl) 
        : IRequest<Result<ClientDto>>;
}
