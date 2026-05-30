using Application.Common.Models;
using Application.Features.Clients.Errors;
using Application.Features.Workouts;
using AutoMapper;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.Create
{
    internal class CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateBookingCommand, Result<BookingDto>>
    {
        public async Task<Result<BookingDto>> Handle(CreateBookingCommand request, CancellationToken ct)
        {
            var client = await unitOfWork.ClientRepository.GetByIdAsync(request.ClientId, ct);
            if (client == null)
                return ClientErrors.NotFound;

            var workout = await unitOfWork.WorkoutRepository.GetByIdAsync(request.WorkoutId, ct,
                w => w.Bookings,
                w => w.WorkoutType);

            if (workout == null)
                return WorkoutErrors.NotFound;

            if (!workout.HasFreeSlots())
                return BookingErrors.NoFreeSlots;

            if (workout.Status != WorkoutStatus.Available)
                return BookingErrors.WorkoutNotAvailable;

            var existing = await unitOfWork.BookingRepository.FirstOrDefaultAsync(
                b => b.ClientId == request.ClientId &&
                     b.WorkoutId == request.WorkoutId &&
                     b.Status == BookingStatus.Active, ct);

            if (existing != null)
                return BookingErrors.AlreadyBooked;

            var booking = new Booking(request.ClientId, request.WorkoutId);

            await unitOfWork.BookingRepository.AddAsync(booking, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return mapper.Map<BookingDto>(booking);
        }
    }
}
