using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.AssignMembershipToClient
{
    public sealed record AssignMembershipToClientCommand(
        int ClientId,
        int MembershipTypeId
    ) : IRequest<Result<MembershipDto>>;
}
