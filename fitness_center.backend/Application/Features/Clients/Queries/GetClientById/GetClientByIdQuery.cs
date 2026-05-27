using Application.Common.Models;
using Application.Features.Clients.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Queries.GetClientById
{
    public sealed record GetClientByIdQuery(int id) : IRequest<Result<ClientDto>>;
}
