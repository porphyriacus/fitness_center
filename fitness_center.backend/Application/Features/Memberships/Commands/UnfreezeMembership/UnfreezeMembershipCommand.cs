using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.UnfreezeMembership
{
    public sealed record UnfreezeMembershipCommand(int ClientId) : IRequest<Result>;
}
