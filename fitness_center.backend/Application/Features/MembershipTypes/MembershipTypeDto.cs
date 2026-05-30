using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes
{
    public class MembershipTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? SessionsCount { get; set; }      // null = безлимит
        public int ValidityDays { get; set; }
        public bool CanFreeze { get; set; }
        public int? MaxFreezeDays { get; set; }      // null = нельзя заморозить
        public bool IsUnlimited => SessionsCount == null;
    }
}
