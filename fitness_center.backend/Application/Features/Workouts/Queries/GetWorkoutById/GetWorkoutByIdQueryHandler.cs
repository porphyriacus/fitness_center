using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Errors;
using Application.Features.Trainers.Queries.GetTrainerById;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Queries.GetWorkoutById
{
    internal class GetWorkoutByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : IRequestHandler<GetWorkoutByIdQuery, Result<WorkoutDto>>
    {
        public async Task<Result<WorkoutDto>> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
        {
            var tr = await unitOfWork.WorkoutRepository.GetByIdAsync(
                request.id
                , cancellationToken
                , w => w.Bookings
                , w => w.WorkoutType
                , w => w.Trainer
            );
            if (tr == null)
            {
                return WorkoutErrors.NotFound;
            }

            return mapper.Map<WorkoutDto>(tr);
        }
    }
}
