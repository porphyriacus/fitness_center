using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.MarkNoShow
{
    internal class MarkNoShowCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<MarkNoShowCommand, Result>
    {
        public async Task<Result> Handle(MarkNoShowCommand request, CancellationToken ct)
        {
            var booking = await unitOfWork.BookingRepository.GetByIdAsync(request.BookingId, ct);

            if (booking == null)
                return BookingErrors.NotFound;

            try
            {
                booking.MarkAsNotCome();
                await unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (WorkoutException ex)
            {
                return Result.Failure(Error.Validation("Booking.MarkFailed", ex.Message));
            }
        }
    }
}
