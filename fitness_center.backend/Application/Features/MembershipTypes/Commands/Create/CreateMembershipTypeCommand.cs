using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Create
{
    public sealed record CreateMembershipTypeCommand(
        string Name,
        string? Description,
        decimal Price,
        int? SessionsCount,
        int ValidityDays,
        bool CanFreeze,
        int? MaxFreezeDays
    ) : IRequest<Result<MembershipTypeDto>>;
}
