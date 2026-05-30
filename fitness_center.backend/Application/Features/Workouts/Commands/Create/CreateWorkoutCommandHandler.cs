using Application.Common.Models;
using Application.Features.Workouts;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Create
{
    internal class CreateWorkoutCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : IRequestHandler<CreateWorkoutCommand, Result<WorkoutDto>>
    {
        public async Task<Result<WorkoutDto>> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workoutType = await unitOfWork.WorkoutTypeRepository.GetByIdAsync(request.WorkoutTypeId, cancellationToken);
            if (workoutType == null)
                return WorkoutErrors.WorkoutTypeNotFound;

            Trainer trainer = await unitOfWork.TrainerRepository.GetByIdAsync(request.TrainerId, cancellationToken);
            if(trainer == null)
                return WorkoutErrors.TrainerNotFound;

            var workout = new Workout(
                workoutType
                , trainer
                , request.StartAt
            );
            await unitOfWork.WorkoutRepository.AddAsync( workout , cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<WorkoutDto>( workout );

        }
    }
}
