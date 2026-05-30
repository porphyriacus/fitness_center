using Application.Common.Models;
using Application.Features.Workouts.Commands.Close;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts.Commands.Delete
{
    internal class DeleteWorkoutCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkoutCommand, Result>
    {
        public async Task<Result> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
        {
            Workout? workout = await unitOfWork.WorkoutRepository.GetByIdAsync(request.id, cancellationToken);
            if (workout == null)
                return WorkoutErrors.NotFound;

            await unitOfWork.WorkoutRepository.DeleteAsync(workout, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
