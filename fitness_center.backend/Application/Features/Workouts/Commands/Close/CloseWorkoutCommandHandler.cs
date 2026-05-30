using Application.Common.Models;
using Application.Features.Workouts.Commands.Cancel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Close
{
    internal class CloseWorkoutCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CloseWorkoutCommand, Result>
    {
        public async Task<Result> Handle(CloseWorkoutCommand request, CancellationToken cancellationToken)
        {
            Workout? workout = await unitOfWork.WorkoutRepository.GetByIdAsync(request.id, cancellationToken);
            if (workout == null)
                return WorkoutErrors.NotFound;

            workout.Close();

            await unitOfWork.WorkoutRepository.UpdateAsync(workout, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
