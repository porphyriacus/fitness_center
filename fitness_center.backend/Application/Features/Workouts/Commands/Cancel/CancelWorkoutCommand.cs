using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Cancel
{

    public sealed record CancelWorkoutCommand(int id) : IRequest<Result>;

}
