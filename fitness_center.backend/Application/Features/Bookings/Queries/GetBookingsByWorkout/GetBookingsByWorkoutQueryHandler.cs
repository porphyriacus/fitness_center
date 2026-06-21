using Application.Common.Models;
using AutoMapper;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetBookingsByWorkout
{
    internal class GetBookingsByWorkoutQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetBookingsByWorkoutQuery, Result<IReadOnlyList<BookingDto>>>
    {
        public async Task<Result<IReadOnlyList<BookingDto>>> Handle(GetBookingsByWorkoutQuery request, CancellationToken ct)
        {
            var filters = new List<Expression<Func<Booking, bool>>>
            {
               b => b.WorkoutId == request.WorkoutId
            };
            var bookings = await unitOfWork.BookingRepository.ListAsync(
               
                q => q.OrderBy(b => b.BookedAt),
                filters,
                ct,
                b => b.Client,
                b => b.Workout,                    
                b => b.Workout.WorkoutType,      
                b => b.Workout.Trainer);

            var dtos = mapper.Map<List<BookingDto>>(bookings);
            return Result<IReadOnlyList<BookingDto>>.Ok(dtos);
        }
    }
}
