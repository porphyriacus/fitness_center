using Application.Common.Models;
using Application.Features.Bookings;
using Application.Features.Bookings.Queries;
using AutoMapper;
using Core.Abstractions;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetClientBookingsHistory
{
    internal class GetClientBookingsHistoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetClientBookingsHistoryQuery, Result<IReadOnlyList<BookingDto>>>
    {
        public async Task<Result<IReadOnlyList<BookingDto>>> Handle(GetClientBookingsHistoryQuery request, CancellationToken ct)
        {
            var filters = new List<Expression<Func<Booking, bool>>>
            {
                 b => b.ClientId == request.ClientId &&
                     (b.Status == BookingStatus.Completed || b.Status == BookingStatus.NotCome)
            };
            var bookings = await unitOfWork.BookingRepository.ListAsync(
               
                q => q.OrderByDescending(b => b.Workout.StartsAt),
                filters,
                ct,
                b => b.Workout,
                b => b.Workout.WorkoutType,
                b => b.Workout.Trainer);

            var dtos = mapper.Map<List<BookingDto>>(bookings);
            return Result<IReadOnlyList<BookingDto>>.Ok(dtos);
        }
    }
}
