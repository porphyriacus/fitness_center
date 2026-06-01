using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Queries.GetClientMembership
{
    internal class GetClientMembershipQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetClientMembershipQuery, Result<MembershipDto?>>
    {
        public async Task<Result<MembershipDto?>> Handle(GetClientMembershipQuery request, CancellationToken ct)
        {
            var membership = await unitOfWork.MembershipRepository.FirstOrDefaultAsync(
                m => m.ClientId == request.ClientId && !m.IsFinished, ct);

            if (membership == null)
                return Result<MembershipDto?>.Ok(null);

            var membershipType = await unitOfWork.MembershipTypeRepository.GetByIdAsync(membership.MembershipTypeId, ct);

            
            var dto = new MembershipDto
            {
                Id = membership.Id,
                ClientId = membership.ClientId,
                MembershipTypeName = membershipType?.Name ?? string.Empty,
                ActivatedDate = membership.ActivatedDate,
                ExpireDate = membership.ExpireDate,
                SessionsLeft = membership.SessionsLeft,
                IsFrozen = membership.IsFrozen,
                FrozenUntil = membership.FrozenUntil,
                IsFinished = membership.IsFinished
            };

            return Result<MembershipDto?>.Ok(dto);
        }
    }
}
