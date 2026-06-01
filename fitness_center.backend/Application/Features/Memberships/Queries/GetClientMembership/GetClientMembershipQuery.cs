using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Queries.GetClientMembership
{
    public sealed record GetClientMembershipQuery(int ClientId) : IRequest<Result<MembershipDto?>>;
}
