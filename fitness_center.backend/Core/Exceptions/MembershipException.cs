using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class MembershipException : DomainException
    {
        public MembershipException(string message) : base(message) { }
    }

    public class MembershipFreezeException : DomainException
    {
        public MembershipFreezeException(string message) : base(message) { }
    }
    public class MembershipFinishedException : DomainException
    {
        public MembershipFinishedException(string message) : base(message) { }
    }
}
