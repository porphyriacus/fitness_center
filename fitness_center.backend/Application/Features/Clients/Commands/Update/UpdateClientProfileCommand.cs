using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.Update
{
    public sealed record UpdateClientProfileCommand(int Id, string Name, string Surname, string Contact, string? ProfilePhotoUrl) : IRequest<Result>;
}
