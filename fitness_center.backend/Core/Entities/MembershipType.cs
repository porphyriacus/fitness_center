using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MembershipType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }    // "4 занятия" "Безлимит"
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int? SessionsCount { get; private set; }     // null = безлимит
        public int ValidityDays { get; private set; }   // срок действия в днях
        public bool CanFreeze { get; private set; }
        public int? MaxFreezeDays { get; private set; }     // максимальная длительность заморозки иф null = нельзя

        public MembershipType(string? name, decimal price,
                              int? sessionsCount, int validityDays,
                              bool canFreeze, int? maxFreezeDays, string? description = null)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (description?.Length > 500) throw new ArgumentException(nameof(description), "Описание не должно превышать длину в 500 символов");
            if (price < 0) throw new ArgumentException(nameof(price), "Цена абонемента не может быть отрицательной");


            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            SessionsCount = sessionsCount;
            ValidityDays = validityDays;
            CanFreeze = canFreeze;
            MaxFreezeDays = maxFreezeDays;
        }

    }
}
