using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Queries.GetWorkoutById
{
    public sealed record GetWorkoutByIdQuery(int id) : IRequest<Result<WorkoutDto>>;
}
