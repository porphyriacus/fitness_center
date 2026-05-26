using Core.Enums;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Workout : Entity
    {
        private readonly List<Booking> _bookings = new();
        public Trainer Trainer { get; private set; }

        public WorkoutType WorkoutType { get; private set; } 
        public int TrainerId { get; private set; }

        public DateTime StartsAt { get; private set; } 
        public WorkoutStatus Status { get; private set; } 
        public IReadOnlyList<Booking> Bookings => _bookings.AsReadOnly(); 

        private Workout() { }

        public Workout(WorkoutType workoutType, Trainer trainer, DateTime startsAt)
        {
           
            if (workoutType == null)
                throw new ArgumentNullException(nameof(workoutType));
            if (trainer == null)
                throw new ArgumentNullException(nameof(trainer));
            if (startsAt == default)
                throw new ArgumentException("Время начала обязательно", nameof(startsAt));

            WorkoutType = workoutType;
            Trainer = trainer;
            TrainerId = trainer.Id;
            StartsAt = startsAt;
            Status = WorkoutStatus.Available;
        }

        public void ChangeTrainer(Trainer newTrainer)
        {
            // тренера можно заменить только за 24 часа
            if (StartsAt <= DateTime.UtcNow.AddHours(24))
                throw new WorkoutException("Нельзя сменить тренера менее чем за 24 часа");

            Trainer = newTrainer;
            TrainerId = newTrainer.Id;
        }

        public bool CanBeBooked(DateTime now)
        {
            return Status == WorkoutStatus.Available
                   && StartsAt > now
                   && HasFreeSlots();
        }

        public bool HasFreeSlots()
        {
            return GetAvailableSlots() > 0;
        }

        public int GetAvailableSlots()
        {
            var maxParticipants = WorkoutType.DefaultMaxCapacity;
            var activeBookings = _bookings.Count(b => b.IsActive);
            return maxParticipants - activeBookings;
        }


        public void Cancel()
        {
            if (StartsAt <= DateTime.UtcNow)
                throw new WorkoutException("Нельзя отменить уже начавшееся занятие");
            Status = WorkoutStatus.Canceled;
        }

        public void Close() => Status = WorkoutStatus.Closed;
    }
}
