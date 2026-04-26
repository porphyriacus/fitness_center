using Core.Exceptions;
using Core.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    /// <summary>
    /// класс абонемента. активируется при первом посещении занятия
    ///                   связан с клиентом по Id
    /// cтратегия необьходима для реализации поведления абонементов разных типов
    /// </summary>
    public class Membership
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public DateTime? ActivatedDate { get; private set; }
        public DateTime? ExpireDate { get; private set; }
        public int? SessionsLeft { get; private set; }
        public bool IsFrozen { get; private set; }
        public DateTime? FrozenUntil { get; private set; }
        public bool FreezeUsed { get; internal set; }

        private readonly IVisitStrategy _visitStrategy;
        private readonly IFreezeStrategy _freezeStrategy;

        /// <summary>
        /// вызывается фабрикой
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="visitStrategy">обработка посещения занятий</param>
        /// <param name="freezeStrategy">обработка заморозки</param>
        /// <param name="sessionsLeft">сколько осталось занятий, если null то безлимит</param>
        internal Membership(
            Guid clientId,
            IVisitStrategy visitStrategy,
            IFreezeStrategy freezeStrategy,
            int? sessionsLeft = null)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
            SessionsLeft = sessionsLeft;
            _visitStrategy = visitStrategy;
            _freezeStrategy = freezeStrategy;
            ActivatedDate = null;
            ExpireDate = null;
            IsFrozen = false;
            FreezeUsed = false;
        }

        internal void SetValidityDays(int days)
        {
            // длительность считается с начала активации
            _validityDays = days;
        }
        private int _validityDays;

        // активация при первом посещении
        private void Activate()
        {
            if (ActivatedDate == null)
            {
                ActivatedDate = DateTime.UtcNow;
                ExpireDate = ActivatedDate.Value.AddDays(_validityDays);
            }
        }

        public void Visit()
        {
            if (!_visitStrategy.CanVisit(this))
                throw new MembershipException("Посещение невозможно: абонемент неактивен");

            Activate(); 

            _visitStrategy.ProcessVisit(this);
        }


        public void Freeze(TimeSpan duration)
        {
            if (!_freezeStrategy.CanFreeze(this))
                throw new MembershipException("Заморозка недоступна для этого абонемента");

            if (IsFrozen)
                throw new MembershipException("Абонемент уже заморожен");

            _freezeStrategy.ApplyFreeze(this, duration);
            IsFrozen = true;
            FrozenUntil = DateTime.UtcNow.Add(duration);
        }

        public void Unfreeze()
        {
            if (!IsFrozen) return;
            IsFrozen = false;
            FrozenUntil = null;
        }
        
        //  пока так вообще сомнительно 
        public bool IsActiveForBooking()
        {
            // автоматическое снятие заморозки если срок истёк
            if (IsFrozen && FrozenUntil <= DateTime.UtcNow)
                Unfreeze();

            if (IsFrozen) return false;
            if (ExpireDate.HasValue && DateTime.UtcNow > ExpireDate.Value) return false;
            if (SessionsLeft.HasValue && SessionsLeft.Value <= 0) return false;
            return true;
        }

        // для стратегий
        internal void ReduceSessionsLeft(int count)
        {
            if (SessionsLeft.HasValue)
                SessionsLeft -= count;
        }

        internal void ShiftExpiration(TimeSpan duration)
        {
            if (ExpireDate.HasValue)
                ExpireDate = ExpireDate.Value.Add(duration);
        }
    }
}
