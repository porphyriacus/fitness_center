using Application.Common.Models;
using Application.Features.Trainers.Commands.Delete;
using Application.Features.Trainers.Errors;
using Core.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var filters = new List<Expression<Func<Workout, bool>>>
            {
                w => w.WorkoutType.Id == request.id
            };
            var workouts = await unitOfWork.WorkoutRepository.ListAsync(
               null,
               filters,
               cancellationToken
            );

            foreach (var workout in workouts)
            {
                await unitOfWork.WorkoutRepository.DeleteAsync(workout, cancellationToken);
            }
            await unitOfWork.WorkoutTypeRepository.DeleteAsync(workoutType, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
