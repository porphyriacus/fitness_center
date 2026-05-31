using Application.Common.Models;
using Application.Features.WorkoutTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Update
{
    public sealed record UpdateMembershipTypeCommand(
        int Id,
        string Name,
        string? Description,
        decimal Price
    ) : IRequest<Result<MembershipTypeDto>>;
}
