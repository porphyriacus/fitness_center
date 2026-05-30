using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.MarkAttendance
{
    public sealed record MarkAttendanceCommand(int BookingId) : IRequest<Result>;
}
