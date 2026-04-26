using Core.Entities;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Strategies
{
    public interface IFreezeStrategy
    {
        bool CanFreeze(Membership membership);
        void ApplyFreeze(Membership membership, TimeSpan duration);
    }

    public class NoFreezeStrategy : IFreezeStrategy
    {
        public bool CanFreeze(Membership membership) => false;
        public void ApplyFreeze(Membership membership, TimeSpan duration)
            => throw new MembershipException("Этот абонемент нельзя заморозить");
    }

    // одноразовая заморозка с ограничением максимума заморозки
    public class OneTimeFreezeStrategy : IFreezeStrategy
    {
        private readonly TimeSpan _maxDuration;

        public OneTimeFreezeStrategy(TimeSpan maxDuration)
        {
            _maxDuration = maxDuration;
        }

        public bool CanFreeze(Membership membership)
        {
            return membership.ActivatedDate != null && !membership.FreezeUsed;
        }

        public void ApplyFreeze(Membership membership, TimeSpan duration)
        {
            if (membership.ActivatedDate == null)
                throw new MembershipException("Нельзя заморозить неактивированный абонемент");
            if (membership.FreezeUsed)
                throw new MembershipException("Заморозка уже использована");
            if (duration > _maxDuration)
                throw new MembershipException($"Максимальная длительность заморозки: {_maxDuration.Days} дней");

            membership.ShiftExpiration(duration);
            membership.FreezeUsed = true;
        }
    }
}
