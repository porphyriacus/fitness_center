using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MembershipType : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int? SessionsCount { get; private set; }     // null = безлимит
        public int ValidityDays { get; private set; }
        public bool CanFreeze { get; private set; }
        public int? MaxFreezeDays { get; private set; }     // null = нельзя заморозить

        private MembershipType() { } // для EF Core

        public MembershipType(
            string name,
            decimal price,
            int? sessionsCount,
            int validityDays,
            bool canFreeze,
            int? maxFreezeDays,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым", nameof(name));

            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной", nameof(price));

            if (validityDays <= 0)
                throw new ArgumentException("Срок действия должен быть положительным", nameof(validityDays));

            if (sessionsCount.HasValue && sessionsCount.Value <= 0)
                throw new ArgumentException("Количество занятий должно быть положительным", nameof(sessionsCount));

            if (!canFreeze && maxFreezeDays != null)
                throw new ArgumentException("Если заморозка недоступна, MaxFreezeDays должен быть null", nameof(maxFreezeDays));

            if (canFreeze && (maxFreezeDays == null || maxFreezeDays <= 0))
                throw new ArgumentException("При возможности заморозки MaxFreezeDays должен быть больше 0", nameof(maxFreezeDays));

            if (description?.Length > 500)
                throw new ArgumentException("Описание не должно превышать 500 символов", nameof(description));

            Name = name;
            Description = description;
            Price = price;
            SessionsCount = sessionsCount;
            ValidityDays = validityDays;
            CanFreeze = canFreeze;
            MaxFreezeDays = maxFreezeDays;
        }

        public bool IsUnlimited => SessionsCount == null;
        public bool HasFreeze => CanFreeze && MaxFreezeDays.HasValue;
    }
}