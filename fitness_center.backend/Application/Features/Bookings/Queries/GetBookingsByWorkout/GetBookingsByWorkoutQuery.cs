using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetBookingsByWorkout
{
    public sealed record GetBookingsByWorkoutQuery(int WorkoutId) : IRequest<Result<IReadOnlyList<BookingDto>>>;
}
