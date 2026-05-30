using Application.Common.Behaviors.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships
{
    public static class MembershipErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Membership.NotFound", "Абонемент не найден");

        public static readonly Error AlreadyActive =
            Error.Conflict("Membership.AlreadyActive", "У клиента уже есть активный абонемент");

        public static readonly Error CannotFreeze =
            Error.Validation("Membership.CannotFreeze", "Этот абонемент нельзя заморозить");

        public static readonly Error AlreadyFrozen =
            Error.Conflict("Membership.AlreadyFrozen", "Абонемент уже заморожен");
    }
}
