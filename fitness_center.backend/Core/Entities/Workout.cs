using Core.Enums;
using Core.Exceptions;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Workout
    {
        public Guid Id { get; private set; }
        public WorkoutType? WorkoutType { get; private set; }
        public Guid TrainerId { get; private set; }
        public DateTime? StartsAt { get; private set; }
        public WorkoutStatus? Status { get; private set; }

        public Workout(WorkoutType? workoutType, Guid trainerId, DateTime? startsAt)
        {
            if (workoutType == null)
                throw new ArgumentNullException(nameof(workoutType));
            if (trainerId == Guid.Empty)
                throw new ArgumentException("Тренер обязателен", nameof(trainerId));
            if (startsAt == default)
                throw new ArgumentException("Время начала обязательно", nameof(startsAt));

            Id = Guid.NewGuid();
            WorkoutType = workoutType;
            TrainerId = trainerId;
            StartsAt = startsAt;
            Status = WorkoutStatus.Available;


        }
        public bool CanBeBooked(DateTime now)
        {
            return Status == WorkoutStatus.Available && StartsAt > now;
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
