using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes
{
    public static class WorkoutTypeErrors
    {
        
        public static Error NotFound =
            Error.NotFound("WorkoutType.NotFound", "Тип занятия не найден");
        public static Error DuplicateName =
            Error.NotFound("WorkoutType.DuplicateName", "Такой тип занятия уже существует. Придумайте другое имя");
    }
}