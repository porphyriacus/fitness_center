using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workout
{
    public class WorkoutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;           // из WorkoutType.Name
        public string WorkoutTypeName { get; set; } = string.Empty;
        public int WorkoutTypeId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public int TrainerId { get; set; }
        public DateTime StartsAt { get; set; }
        public string Status { get; set; } = string.Empty;         // "Available", "Canceled", "Closed"
        public int MaxParticipants { get; set; }                   // из WorkoutType.DefaultMaxCapacity
        public int CurrentBookingsCount { get; set; }              // количество записавшихся
        public int AvailableSlots { get; set; }                    // свободные места
        public bool CanBeBooked { get; set; }                      // можно ли записаться
    }
}
