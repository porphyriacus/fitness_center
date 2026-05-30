using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings
{
    public static class BookingErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Booking.NotFound", "Запись не найдена");

        public static readonly Error NoFreeSlots =
            Error.Conflict("Booking.NoFreeSlots", "Нет свободных мест на эту тренировку");

        public static readonly Error AlreadyBooked =
            Error.Conflict("Booking.AlreadyBooked", "Вы уже записаны на эту тренировку");

        public static readonly Error NoActiveMembership =
            Error.Failure("Booking.NoActiveMembership", "У вас нет активного абонемента");

        public static readonly Error CannotCancelLate =
            Error.Validation("Booking.CannotCancelLate", "Нельзя отменить запись менее чем за 2 часа до начала");

        public static readonly Error WorkoutNotAvailable =
            Error.Conflict("Booking.WorkoutNotAvailable", "Тренировка недоступна для записи");

        public static readonly Error InvalidStatus =
            Error.Validation("Booking.InvalidStatus", "Нельзя отметить посещение для этой записи");
    }
}
