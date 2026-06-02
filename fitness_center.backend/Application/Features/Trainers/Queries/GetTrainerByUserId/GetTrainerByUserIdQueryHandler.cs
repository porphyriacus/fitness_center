using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Queries.GetTrainerByUserId
{
    internal class GetTrainerByUserIdQueryHandler : IRequestHandler<GetTrainerByUserIdQuery, Result<TrainerDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTrainerByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TrainerDto?>> Handle(GetTrainerByUserIdQuery request, CancellationToken cancellationToken)
        {
            var trainer = await _unitOfWork.TrainerRepository.FirstOrDefaultAsync(
                           t => t.IdentityUserId == request.IdentityUserId,
                           cancellationToken);

            if (trainer == null)
                return Result<TrainerDto?>.Ok(null);

            var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(trainer.SpecializationId, cancellationToken);

            var dto = _mapper.Map<TrainerDto>(trainer);
            dto.Specialization = specialization?.Name ?? string.Empty;

            return Result<TrainerDto?>.Ok(dto);
        }
    }
}
