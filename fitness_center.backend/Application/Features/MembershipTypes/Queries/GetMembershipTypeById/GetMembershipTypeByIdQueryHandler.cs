using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Queries.GetMembershipTypeById
{
    internal class GetMembershipTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetMembershipTypeByIdQuery, Result<MembershipTypeDto>>
    {
        public async Task<Result<MembershipTypeDto>> Handle(GetMembershipTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var membershipType = await unitOfWork.MembershipTypeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (membershipType == null)
                return MembershipTypeErrors.NotFound;

            return mapper.Map<MembershipTypeDto>(membershipType);
        }
    }
}
