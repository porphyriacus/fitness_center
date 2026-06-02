using Application.Common.Behaviors.Errors;
using Application.Common.Models;
using Core.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.FreezeMembership
{
    internal class FreezeMembershipCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<FreezeMembershipCommand, Result>
    {
        public async Task<Result> Handle(FreezeMembershipCommand request, CancellationToken ct)
        {
            var membership = await unitOfWork.MembershipRepository.FirstOrDefaultAsync(
                m => m.ClientId == request.ClientId && !m.IsFinished, ct);

            if (membership == null)
                return MembershipErrors.NotFound;

            try
            {
                membership.Freeze(request.Duration);
                await unitOfWork.SaveChangesAsync(ct);
                return Result.Success();
            }
            catch (MembershipFreezeException ex)
            {
                return Result.Failure(Error.Validation("Membership.CannotFreeze", ex.Message));
            }
            catch (MembershipException ex)
            {
                return Result.Failure(Error.Failure("Membership.FreezeError", ex.Message));
            }

        }
    }
}
