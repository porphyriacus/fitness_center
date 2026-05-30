using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetBookingById
{
    internal class GetBookingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
    {
        public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken ct)
        {
            var booking = await unitOfWork.BookingRepository.GetByIdAsync(
                request.Id,
                ct,
                b => b.Workout,
                b => b.Workout.WorkoutType,
                b => b.Workout.Trainer,
                b => b.Client);

            if (booking == null)
                return BookingErrors.NotFound;

            var dto = mapper.Map<BookingDto>(booking);
            return Result<BookingDto>.Ok(dto);
        }
    }
}
