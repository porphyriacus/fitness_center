using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Application.Features.Memberships
//{
//    public class MembershipDto
//    {
//        public int Id { get; set; }
//        public int ClientId { get; set; }
//        public string MembershipTypeName { get; set; } = string.Empty;
//        public DateTime? ActivatedDate { get; set; }
//        public DateTime? ExpireDate { get; set; }
//        public int? SessionsLeft { get; set; }
//        public bool IsFrozen { get; set; }
//        public DateTime? FrozenUntil { get; set; }
//        public bool IsFinished { get; set; }
//        public bool IsUnlimited => SessionsLeft == null;
//    }
//}
namespace Application.Features.Memberships
{
    public class MembershipDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string MembershipTypeName { get; set; } = string.Empty;
        public int? SessionsCount { get; set; }           // всего занятий в типе
        public int ValidityDays { get; set; }              // срок действия в днях
        public bool CanFreeze { get; set; }                // можно ли заморозить
        public int? MaxFreezeDays { get; set; }            // макс дней заморозки
        public DateTime? ActivatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? SessionsLeft { get; set; }
        public bool FreezeUsed { get; set; }
        public bool IsFrozen { get; set; }
        public DateTime? FrozenUntil { get; set; }
        public bool IsFinished { get; set; }
        public bool IsUnlimited => SessionsLeft == null;
        public string StatusText => GetStatusText();
        public string StatusColor => GetStatusColor();

        private string GetStatusText()
        {
            if (IsFinished) return "Закончился";
            if (IsFrozen) return $"Заморожен до {FrozenUntil:dd.MM.yyyy}";
            if (!ActivatedDate.HasValue) return "Ожидает активации";
            return "Активен";
        }

        private string GetStatusColor()
        {
            if (IsFinished) return "danger";
            if (IsFrozen) return "warning";
            if (!ActivatedDate.HasValue) return "secondary";
            return "success";
        }
    }
}