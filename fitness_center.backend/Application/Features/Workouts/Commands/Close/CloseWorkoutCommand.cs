using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Close
{
    public sealed record CloseWorkoutCommand(int id) : IRequest<Result>;
}
