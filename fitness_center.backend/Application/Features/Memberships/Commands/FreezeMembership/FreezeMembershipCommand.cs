using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.FreezeMembership
{
    public sealed record FreezeMembershipCommand(
        int ClientId,
        TimeSpan Duration
    ) : IRequest<Result>;
}
