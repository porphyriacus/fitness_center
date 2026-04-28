using Core.Enums;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid WorkoutId { get; private set; }
        public BookingStatus Status { get; private set; }


        public Booking(Guid clientId, Guid workoutId)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
            WorkoutId = workoutId;
            Status = BookingStatus.Applied;
        }
        public void Cancel()
        {
            if (Status == BookingStatus.Canceled)
                return;
            Status = BookingStatus.Canceled;
        }
        public void ClientDidNotCome()
        {
            if (Status != BookingStatus.Applied)
                throw new WorkoutException("Только подтверждённую запись можно отметить как неявку");
            Status = BookingStatus.NotCome;
        }
    }
}
