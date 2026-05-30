using Application.Common.Models;
using Application.Features.Trainers.Commands.Delete;
using Application.Features.Trainers.Errors;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Cancel
{

    internal class CancelWorkoutCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CancelWorkoutCommand, Result>
    {
        public async Task<Result> Handle(CancelWorkoutCommand request, CancellationToken cancellationToken)
        {
            Workout? workout = await unitOfWork.WorkoutRepository.GetByIdAsync(request.id, cancellationToken);
            if (workout == null)
                return WorkoutErrors.NotFound;

            try
            {
                workout.Cancel();
            }
            catch (WorkoutException ex)
            {
                return WorkoutErrors.WorkoutCantBeCancel(ex.Message);
            }

            await unitOfWork.WorkoutRepository.UpdateAsync(workout, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
