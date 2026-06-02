using Core.Enums;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Booking : Entity
    {
        public int ClientId { get; private set; }
        public Client Client { get; private set; } 
        public int WorkoutId { get; private set; }
        public Workout Workout { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime BookedAt { get; private set; }
        public DateTime? CancelledAt { get; private set; }


        public bool IsActive => Status == BookingStatus.Active;
        public bool IsCanceled => Status == BookingStatus.Cancelled;
        private Booking() { } // для EF Core

        public Booking(int clientId, int workoutId)
        {
            if (clientId <= 0)
                throw new ArgumentException("ID клиента обязателен", nameof(clientId));
            if (workoutId <= 0)
                throw new ArgumentException("ID тренировки обязателен", nameof(workoutId));

            ClientId = clientId;
            WorkoutId = workoutId;
            Status = BookingStatus.Active;
            BookedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
                return;

            if (Status == BookingStatus.Completed)
                throw new WorkoutException("Нельзя отменить уже проведённую тренировку");

            Status = BookingStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            if (Status != BookingStatus.Active)
                throw new WorkoutException("Только активную запись можно отметить как посещённую");

            Status = BookingStatus.Completed;
        }

        public void MarkAsNotCome()
        {
            if (Status != BookingStatus.Active)
                throw new WorkoutException("Только активную запись можно отметить как неявку");

            Status = BookingStatus.NotCome;
        }
    }

}
