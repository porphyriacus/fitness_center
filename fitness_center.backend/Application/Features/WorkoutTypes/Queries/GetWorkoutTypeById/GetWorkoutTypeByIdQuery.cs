using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Queries.GetWorkoutTypeById
{
    public sealed record GetWorkoutTypeByIdQuery(int id) : IRequest<Result<WorkoutTypeDto>>;
}
