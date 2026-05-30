using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.UnfreezeMembership
{
    internal class UnfreezeMembershipCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<UnfreezeMembershipCommand, Result>
    {
        public async Task<Result> Handle(UnfreezeMembershipCommand request, CancellationToken ct)
        {
            var membership = await unitOfWork.MembershipRepository.FirstOrDefaultAsync(
                m => m.ClientId == request.ClientId && !m.IsFinished, ct);

            if (membership == null)
                return MembershipErrors.NotFound;

            try
            {
                membership.Unfreeze();
                await unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (MembershipFreezeException ex)
            {
                return Result.Failure(Error.Validation("Membership.CannotUnfreeze", ex.Message));
            }
        }
    }
}
