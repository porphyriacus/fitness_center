using Application.Common.Models;
using AutoMapper;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetBookingsByWorkout
{
    internal class GetBookingsByWorkoutQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetBookingsByWorkoutQuery, Result<IReadOnlyList<BookingDto>>>
    {
        public async Task<Result<IReadOnlyList<BookingDto>>> Handle(GetBookingsByWorkoutQuery request, CancellationToken ct)
        {
            var bookings = await unitOfWork.BookingRepository.ListAsync(
                b => b.WorkoutId == request.WorkoutId && b.Status == BookingStatus.Active,
                q => q.OrderBy(b => b.BookedAt),
                ct,
                b => b.Client);

            var dtos = mapper.Map<List<BookingDto>>(bookings);
            return Result<IReadOnlyList<BookingDto>>.Ok(dtos);
        }
    }
}
