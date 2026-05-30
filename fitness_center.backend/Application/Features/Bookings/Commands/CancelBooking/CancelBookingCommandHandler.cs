using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.CancelBooking
{
    internal class CancelBookingCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CancelBookingCommand, Result>
    {
        public async Task<Result> Handle(CancelBookingCommand request, CancellationToken ct)
        {
            var booking = await unitOfWork.BookingRepository.GetByIdAsync(request.BookingId, ct,
                b => b.Workout);

            if (booking == null)
                return BookingErrors.NotFound;

            try
            {
                booking.Cancel();
                await unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (WorkoutException ex)
            {
                return Result.Failure(Error.Validation("Booking.CancelFailed", ex.Message));
            }
        }
    }
}
