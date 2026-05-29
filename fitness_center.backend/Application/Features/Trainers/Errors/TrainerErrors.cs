using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Errors
{
    public static class TrainerErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Trainer.NotFound", "Тренер не найден");
        public static readonly Error SpecializationNotFound =
            Error.NotFound("Trainer.NotFound", "Специализация не найдена");
    }
}
