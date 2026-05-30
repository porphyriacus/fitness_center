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
    public sealed record UpdateWorkoutTypeCommand(
        int Id,
        string? Description,
        string? Color,
        decimal Price
    ) : IRequest<Result<WorkoutTypeDto>>;
}
