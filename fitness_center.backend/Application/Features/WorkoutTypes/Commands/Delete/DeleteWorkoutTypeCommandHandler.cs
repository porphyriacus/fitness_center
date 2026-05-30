using Application.Common.Models;
using Application.Features.Trainers.Commands.Delete;
using Application.Features.Trainers.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Delete
{
    internal class DeleteWorkoutTypeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkoutTypeCommand, Result>
    {
        public async Task<Result> Handle(DeleteWorkoutTypeCommand request, CancellationToken cancellationToken)
        {
            var workoutType = await unitOfWork.WorkoutTypeRepository.GetByIdAsync(request.id, cancellationToken);
            if (workoutType == null)
            {
                return WorkoutTypeErrors.NotFound;
            }

            await unitOfWork.WorkoutTypeRepository.DeleteAsync(workoutType, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
