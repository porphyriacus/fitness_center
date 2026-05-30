using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.MarkNoShow
{
    public sealed record MarkNoShowCommand(int BookingId) : IRequest<Result>;
}
