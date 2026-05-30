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

namespace Application.Features.WorkoutTypes.Queries.GetWorkoutTypeById
{
    internal class GetWorkoutTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetWorkoutTypeByIdQuery, Result<WorkoutTypeDto>>
    {
        public async Task<Result<WorkoutTypeDto>> Handle(GetWorkoutTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var wt = await unitOfWork.WorkoutTypeRepository.GetByIdAsync(request.id, cancellationToken);
            if (wt == null)
            {
                return WorkoutTypeErrors.NotFound;
            }

            return mapper.Map<WorkoutTypeDto>(wt);
        }
    }
}
