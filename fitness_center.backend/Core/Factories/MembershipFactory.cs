using Core.Entities;
using Core.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories
{
    public static class MembershipFactory
    {
        public static Membership CreateFromType(Guid clientId, MembershipType type)
        {
            IVisitStrategy visitStrategy = type.SessionsCount.HasValue
                ? new LimitedVisitStrategy()
                : new UnlimitedVisitStrategy();

            IFreezeStrategy freezeStrategy;
            if (!type.CanFreeze || type.MaxFreezeDays == null)
                freezeStrategy = new NoFreezeStrategy();
            else
                freezeStrategy = new OneTimeFreezeStrategy(TimeSpan.FromDays(type.MaxFreezeDays.Value));

            var membership = new Membership(
                clientId,
                visitStrategy,
                freezeStrategy,
                type.ValidityDays,
                type.SessionsCount
            );
            return membership;
        }
    }
}
