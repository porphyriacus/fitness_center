using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workouts
{
    public static class WorkoutErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Workout.NotFound", "Тренеровка не найдена");
        public static readonly Error TrainerNotFound =
            Error.NotFound("Workout.TrainerNotFound", "Тренер не найден");
        public static readonly Error WorkoutTypeNotFound =
            Error.NotFound("Workout.WorkoutTypeNotFound", "Тип занятия не найден");
        public static Error WorkoutCantBeCancel(string message) =>
            Error.Conflict("Workout.WorkoutCantBeCancel", message);

        public static Error CantEdit(string message) =>
            Error.Conflict("Workout.CantEdit", message);

    }
}
