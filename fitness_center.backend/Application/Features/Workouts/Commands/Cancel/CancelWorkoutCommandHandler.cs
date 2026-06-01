using Application.Common.Models;
using Application.Features.Trainers.Commands.Delete;
using Application.Features.Trainers.Errors;
using Core.Abstractions;
using Core.Enums;
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
            var workout = await unitOfWork.WorkoutRepository.GetByIdAsync(
                request.id,
                cancellationToken,
                w => w.Bookings);

            if (workout == null)
                return WorkoutErrors.NotFound;

            try
            {
                workout.Cancel();

                var activeBookings = workout.Bookings.Where(b => b.Status == BookingStatus.Active).ToList();
                foreach (var booking in activeBookings)
                {
                    booking.Cancel();
                }
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
