using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetBookingById
{
    public sealed record GetBookingByIdQuery(int Id) : IRequest<Result<BookingDto>>;
}
