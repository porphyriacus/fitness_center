using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Queries.GetMembershipTypeById
{
    public sealed record GetMembershipTypeByIdQuery(int Id) : IRequest<Result<MembershipTypeDto>>;
}
