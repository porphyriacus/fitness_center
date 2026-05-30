using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Delete
{
    internal class DeleteMembershipTypeCommandHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteMembershipTypeCommand, Result>
    {
        public async Task<Result> Handle(DeleteMembershipTypeCommand request, CancellationToken cancellationToken)
        {
            var membershipType = await unitOfWork.MembershipTypeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (membershipType == null)
                return MembershipTypeErrors.NotFound;

            await unitOfWork.MembershipTypeRepository.DeleteAsync(membershipType, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
