using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries
{
    public sealed record GetClientBookingsQuery(int ClientId) : IRequest<Result<IReadOnlyList<BookingDto>>>;
}