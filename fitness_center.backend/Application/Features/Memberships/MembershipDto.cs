using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships
{
    public class MembershipDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string MembershipTypeName { get; set; } = string.Empty;
        public DateTime? ActivatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? SessionsLeft { get; set; }
        public bool IsFrozen { get; set; }
        public DateTime? FrozenUntil { get; set; }
        public bool IsFinished { get; set; }
        public bool IsUnlimited => SessionsLeft == null;
    }
}
