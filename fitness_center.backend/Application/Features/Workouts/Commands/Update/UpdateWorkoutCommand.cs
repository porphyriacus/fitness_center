using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Update
{
    public sealed record UpdateWorkoutCommand(int WorkoutId, int TrainerId, DateTime StartAt)
        : IRequest<Result<WorkoutDto>>;
}
