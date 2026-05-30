using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.FreezeMembership
{
    public class FreezeMembershipCommandValidator : AbstractValidator<FreezeMembershipCommand>
    {
        public FreezeMembershipCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.Duration).GreaterThan(TimeSpan.Zero);
        }
    }
}
