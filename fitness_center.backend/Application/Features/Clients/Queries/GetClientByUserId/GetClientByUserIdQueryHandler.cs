using Application.Common.Models;
using Application.Features.Clients.DTOs;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Queries.GetClientByUserId
{
    internal class GetClientByUserIdQueryHandler : IRequestHandler<GetClientByUserIdQuery, Result<ClientDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ClientDto?>> Handle(GetClientByUserIdQuery request, CancellationToken ct)
        {
            var client = await _unitOfWork.ClientRepository.FirstOrDefaultAsync(
                c => c.IdentityUserId == request.IdentityUserId, ct);

            if (client == null)
                return Result<ClientDto?>.Ok(null);

            var dto = _mapper.Map<ClientDto>(client);
            return Result<ClientDto?>.Ok(dto);
        }
    }
}
