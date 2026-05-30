using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes
{

    public static class MembershipTypeErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("MembershipType.NotFound", "Тип абонемента не найден");
        public static Error DuplicateName =
          Error.NotFound("MembershipType.DuplicateName", "Такой тип абонемента уже существует. Придумайте другое имя");
    }
}
