using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Delete
{
    public sealed record DeleteWorkoutTypeCommand(int id) : IRequest<Result>;
}
