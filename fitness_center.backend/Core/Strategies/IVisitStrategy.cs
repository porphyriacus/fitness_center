using Core.Entities;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Strategies
{

    public interface IVisitStrategy
    {
        bool CanVisit(Membership membership);
        void ProcessVisit(Membership membership);
    }
    public class LimitedVisitStrategy : IVisitStrategy
    {
        public bool CanVisit(Membership membership)
        {
            return membership.SessionsLeft > 0 &&
                   (!membership.ExpireDate.HasValue || DateTime.UtcNow <= membership.ExpireDate.Value);
        }

        public void ProcessVisit(Membership membership)
        {
            if (membership.SessionsLeft <= 0)
                throw new MembershipException("Нет доступных занятий");
            membership.ReduceSessionsLeft(1);
        }
    }

    public class UnlimitedVisitStrategy : IVisitStrategy
    {
        public bool CanVisit(Membership membership)
        {
            return !membership.ExpireDate.HasValue || DateTime.UtcNow <= membership.ExpireDate.Value;
        }

        public void ProcessVisit(Membership membership)
        {
            // безлимит потому ничего не делаем
        }
    }
}
