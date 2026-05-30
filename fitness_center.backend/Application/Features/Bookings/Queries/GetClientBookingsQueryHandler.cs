using Application.Common.Models;
using AutoMapper;
using Core.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries
{
    internal class GetClientBookingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetClientBookingsQuery, Result<IReadOnlyList<BookingDto>>>
    {
        public async Task<Result<IReadOnlyList<BookingDto>>> Handle(GetClientBookingsQuery request, CancellationToken ct)
        {
            var bookings = await unitOfWork.BookingRepository.ListAsync(
                b => b.ClientId == request.ClientId && b.Status == BookingStatus.Active,
                q => q.OrderBy(b => b.Workout.StartsAt),
                ct,
                b => b.Workout,
                b => b.Workout.WorkoutType,
                b => b.Workout.Trainer);

            var dtos = mapper.Map<List<BookingDto>>(bookings);
            return Result<IReadOnlyList<BookingDto>>.Ok(dtos);
        }
    }
}
