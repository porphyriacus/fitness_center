using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Create
{
    internal class CreateMembershipTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<CreateMembershipTypeCommand, Result<MembershipTypeDto>>
    {
        public async Task<Result<MembershipTypeDto>> Handle(CreateMembershipTypeCommand request, CancellationToken cancellationToken)
        {
            var existing = await unitOfWork.MembershipTypeRepository.FirstOrDefaultAsync(
                mt => mt.Name == request.Name, cancellationToken);

            if (existing != null)
                return MembershipTypeErrors.DuplicateName;

            var membershipType = new MembershipType(
                request.Name,
                request.Price,
                request.SessionsCount,
                request.ValidityDays,
                request.CanFreeze,
                request.MaxFreezeDays,
                request.Description
            );

            await unitOfWork.MembershipTypeRepository.AddAsync(membershipType, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<MembershipTypeDto>(membershipType);
        }
    }
}
