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
    public class Membership : Entity
    {
        public int ClientId { get; private set; }
        public int MembershipTypeId { get; private set; }

        public DateTime? ActivatedDate { get; private set; }
        public DateTime? ExpireDate { get; private set; }

        public int ValidityDays { get; private set; }
        public int? SessionsLeft { get; private set; }

        public bool IsFrozen { get; private set; }
        public DateTime? FrozenUntil { get; private set; }
        public bool FreezeUsed { get; private set; }

        public bool IsFinished { get; private set; }

        private readonly IVisitStrategy _visitStrategy;
        private readonly IFreezeStrategy _freezeStrategy;

        internal Membership(
            int clientId,
            int membershipTypeId, 
            IVisitStrategy visitStrategy,
            IFreezeStrategy freezeStrategy,
            int validityDays,
            int? sessionsLeft = null)
        {
            if (visitStrategy == null)
                throw new ArgumentNullException(nameof(visitStrategy), "Нужно явно указать стратегию поведения при посещении занятия");
            if (freezeStrategy == null)
                throw new ArgumentNullException(nameof(freezeStrategy), "Нужно явно указать стратегию поведения при заморозке абонемента");
            if (validityDays <= 0)
                throw new ArgumentException("Время действия абонемента должно быть положительным числом", nameof(validityDays));

            ClientId = clientId;
            MembershipTypeId = membershipTypeId; 
            SessionsLeft = sessionsLeft;
            _visitStrategy = visitStrategy;
            _freezeStrategy = freezeStrategy;
            ValidityDays = validityDays;
            ActivatedDate = null;
            ExpireDate = null;
            IsFrozen = false;
            IsFinished = false;
            FreezeUsed = false;
        }


        // активация при первом посещении
        private void Activate()
        {
            if (ActivatedDate == null)
            {
                ActivatedDate = DateTime.UtcNow;
                ExpireDate = ActivatedDate.Value.AddDays(ValidityDays);
            }
        }

        /// <summary>
        /// Логика посещения занятия
        ///     Желательно проверять возможность бронирования методом IsActiveForBooking()
        ///     Если абонемент заморожен, необходимо рпедварительно снять заморозку(Unfreeze)
        /// </summary>
        /// <exception cref="MembershipFinishedException"></exception>
        public void Visit()
        {
            UpdateInfo();
            if (IsFrozen)
                throw new MembershipFreezeException("Для посещения занятия необходимо чтобы абонемент не был заморожен");
            if (!_visitStrategy.CanVisit(this))
            {
                IsFinished = true;
                throw new MembershipFinishedException("Посещение невозможно: абонемент закончился");
            }
            
         
            Activate(); 
            _visitStrategy.ProcessVisit(this);
        }


        public void Freeze(TimeSpan duration)
        {
            if (!_freezeStrategy.CanFreeze(this))
                throw new MembershipFreezeException("Заморозка недоступна для этого абонемента");

            if (IsFrozen)
                throw new MembershipFreezeException("Абонемент уже заморожен");

            _freezeStrategy.ApplyFreeze(this, duration);
     
        }

        public void Unfreeze()
        {
            if (!IsFrozen) return;
            IsFrozen = false;
            ExpireDate += _freezeStrategy.Unfreeze(this);
            FrozenUntil = null;

        }
        
        //  пока так вообще сомнительно 
        /// <summary>
        /// Посещение доступно если:
        ///     1. абонемент заморожен (если истекло время заморозки автоматич размораживается)
        ///     2. aбонемент закончился:
        ///         - время действие абонемента истекло
        ///         - закончились ззанятия
        /// </summary>
        /// <returns>Можно ли обработать посещение</returns>
        public bool IsActiveForBooking()
        {
            UpdateInfo();
            if (IsFrozen) return false;
            if (IsFinished) return false;
            return true;
        }


        /// <summary>
        /// обновление информации о статусе абонемента
        /// </summary>
        public void UpdateInfo()
        {
            if (IsFrozen && FrozenUntil <= DateTime.UtcNow) Unfreeze();
            if (ExpireDate.HasValue && DateTime.UtcNow > ExpireDate.Value) IsFinished = true;
            if (SessionsLeft.HasValue && SessionsLeft.Value <= 0) IsFinished = true;
        }

        // для стратегий
        internal void ReduceSessionsLeft(int count)
        {
            if (SessionsLeft.HasValue)
                SessionsLeft -= count;
        }

        internal void ShiftFrozenExpiration(TimeSpan duration)
        {
            IsFrozen = true;
            FrozenUntil = DateTime.UtcNow.Add(duration);
            FreezeUsed = true;
        }

    }
}
