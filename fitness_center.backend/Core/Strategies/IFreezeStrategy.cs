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
        public TimeSpan Unfreeze(Membership membership);
    }

    public class NoFreezeStrategy : IFreezeStrategy
    {
        public bool CanFreeze(Membership membership) => false;
        public void ApplyFreeze(Membership membership, TimeSpan duration)
            => throw new MembershipFreezeException("Этот абонемент нельзя заморозить");
        public TimeSpan Unfreeze(Membership membership) =>
            throw new MembershipFreezeException("Этот абонемент нельзя заморозить, соответственно и разморозить");
    }

    // одноразовая заморозка с ограничением максимума заморозки
    public class OneTimeFreezeStrategy : IFreezeStrategy
    {
        private readonly TimeSpan _maxDuration;
        private DateTime _startFreeze;
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
            if (membership.IsFinished)
                throw new MembershipException("Нельзя заморозить абонемент который закончился");
            if (membership.FreezeUsed)
                throw new MembershipException("Заморозка уже использована");
            if (duration > _maxDuration)
                throw new MembershipException($"Максимальная длительность заморозки: {_maxDuration.Days} дней");

            _startFreeze = DateTime.UtcNow;

            membership.ShiftFrozenExpiration(duration);
        }
        public TimeSpan Unfreeze(Membership membership)
        {
            TimeSpan freezeduration = DateTime.UtcNow - _startFreeze;
            if (freezeduration <= _maxDuration) return freezeduration;
            return _maxDuration;

        }
    }
}
