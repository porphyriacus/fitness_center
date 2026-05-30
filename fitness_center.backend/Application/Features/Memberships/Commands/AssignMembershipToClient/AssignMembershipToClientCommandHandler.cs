using Application.Common.Models;
using Application.Features.Clients.Errors;
using Application.Features.MembershipTypes;
using AutoMapper;
using Core.Factories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Memberships.Commands.AssignMembershipToClient
{
    internal class AssignMembershipToClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AssignMembershipToClientCommand, Result<MembershipDto>>
    {
        public async Task<Result<MembershipDto>> Handle(AssignMembershipToClientCommand request, CancellationToken ct)
        {
            var client = await unitOfWork.ClientRepository.GetByIdAsync(request.ClientId, ct);
            if (client == null)
                return ClientErrors.NotFound;

            var membershipType = await unitOfWork.MembershipTypeRepository.GetByIdAsync(request.MembershipTypeId, ct);
            if (membershipType == null)
                return MembershipTypeErrors.NotFound;

            // проверка на то есть ли у клиента активный абьонемент 
            var existing = await unitOfWork.MembershipRepository.FirstOrDefaultAsync(
                m => m.ClientId == request.ClientId && !m.IsFinished, ct);
            if (existing != null)
                return MembershipErrors.AlreadyActive;

            var membership = MembershipFactory.CreateFromType(request.ClientId, membershipType);

            await unitOfWork.MembershipRepository.AddAsync(membership, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return mapper.Map<MembershipDto>(membership);
        }
    }

}
