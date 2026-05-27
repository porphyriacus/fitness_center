using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.DeleteClientAvatar
{
    public sealed record DeleteClientAvatarCommand(int id) : IRequest<Result>;

}
