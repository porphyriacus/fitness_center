using Application.Common.Models;
using Application.Features.WorkoutTypes;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Update
{
    internal class UpdateMembershipTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<UpdateMembershipTypeCommand, Result<MembershipTypeDto>>
    {
        public async Task<Result<MembershipTypeDto>> Handle(UpdateMembershipTypeCommand request, CancellationToken ct)
        {
            var membershipType = await unitOfWork.MembershipTypeRepository.GetByIdAsync(request.Id, ct);
            if (membershipType == null)
                return MembershipTypeErrors.NotFound;

            if (membershipType.Name != request.Name)
            {
                var existing = await unitOfWork.MembershipTypeRepository.FirstOrDefaultAsync(
                    mt => mt.Name == request.Name, ct);
                if (existing != null)
                    return MembershipTypeErrors.DuplicateName;
            }

            membershipType.UpdateName(request.Name);
            membershipType.UpdateDescription(request.Description);
            membershipType.UpdatePrice(request.Price);

            await unitOfWork.MembershipTypeRepository.UpdateAsync(membershipType, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return mapper.Map<MembershipTypeDto>(membershipType);
        }
    }
}
