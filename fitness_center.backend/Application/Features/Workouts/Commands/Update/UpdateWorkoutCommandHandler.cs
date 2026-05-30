using Application.Common.Models;
using AutoMapper;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Update
{
    internal class UpdateWorkoutCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<UpdateWorkoutCommand, Result<WorkoutDto>>
    {
        public async Task<Result<WorkoutDto>> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var workout = await unitOfWork.WorkoutRepository.GetByIdAsync(request.WorkoutId, cancellationToken);
            if (workout == null)
                return WorkoutErrors.NotFound;

            Trainer trainer = await unitOfWork.TrainerRepository.GetByIdAsync(request.TrainerId, cancellationToken);
            if (trainer == null)
                return WorkoutErrors.TrainerNotFound;
            try
            {
                workout.ChangeTrainer(trainer);
                workout.ChangeStartTime(request.StartAt);
            }
            catch (WorkoutException ex)
            {
                return WorkoutErrors.CantEdit(ex.Message);
            }


            await unitOfWork.WorkoutRepository.UpdateAsync(workout, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            return mapper.Map<WorkoutDto>(workout);

        }
    }
}