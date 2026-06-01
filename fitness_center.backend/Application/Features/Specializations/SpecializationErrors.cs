using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations
{
    public static class SpecializationErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Specialization.NotFound", "Специализация не найдена");

    }
}
