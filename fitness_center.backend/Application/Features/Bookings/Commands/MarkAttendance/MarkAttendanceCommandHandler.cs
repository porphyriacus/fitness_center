using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.MarkAttendance
{
    internal class MarkAttendanceCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<MarkAttendanceCommand, Result>
    {
        public async Task<Result> Handle(MarkAttendanceCommand request, CancellationToken ct)
        {
            var booking = await unitOfWork.BookingRepository.GetByIdAsync(request.BookingId, ct,
                b => b.Client.Membership,
                b => b.Workout);

            if (booking == null)
                return BookingErrors.NotFound;

            try
            {
                booking.MarkAsCompleted();

                // если есть абонемент то списываем занятие
                if (booking.Client?.Membership != null)
                {
                    var membership = await unitOfWork.MembershipRepository
                        .FirstOrDefaultAsync(m => m.ClientId == booking.ClientId && !m.IsFinished, ct);
                    membership.Visit();
                }

                await unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (WorkoutException ex)
            {
                return Result.Failure(Error.Validation("Booking.MarkFailed", ex.Message));
            }
            catch (MembershipFinishedException ex)
            {
                return Result.Failure(Error.Conflict("Membership.Finished", ex.Message));
            }
            catch (MembershipFreezeException ex)
            {
                return Result.Failure(Error.Validation("Membership.Frozen", ex.Message));
            }
        }
    }
}
