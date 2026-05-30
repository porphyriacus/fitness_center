using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Create
{
    internal class CreateWorkoutTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateWorkoutTypeCommand, Result<WorkoutTypeDto>>
    {
        public async Task<Result<WorkoutTypeDto>> Handle(CreateWorkoutTypeCommand request, CancellationToken cancellationToken)
        {
            var workoutType = new WorkoutType(
                request.Name
                , request.Description
                , request.DefaultDurationMinutes
                , request.DefaultDurationMinutes
                , request.Color
            );

            await unitOfWork.WorkoutTypeRepository.AddAsync( workoutType, cancellationToken );
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<WorkoutTypeDto>( workoutType );
        }
    }
}
