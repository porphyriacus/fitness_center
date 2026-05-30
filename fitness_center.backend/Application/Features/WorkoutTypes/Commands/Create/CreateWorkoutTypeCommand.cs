using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutType.Commands.Create
{
    public sealed record CreateWorkoutTypeCommand(string Name, string Description, int DefaultDurationMinutes, int DefaultMaxCapacity, string? Color) 
        : IRequest<Result<WorkoutTypeDto>>;
}
