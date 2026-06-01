using Application.Common.Models;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations.Queries.GetSpecializationsList
{
    internal class GetSpecializationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetSpecializationsQuery, Result<IReadOnlyList<SpecializationDto>>>
    {
        public async Task<Result<IReadOnlyList<SpecializationDto>>> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
        {

            var specializations = await unitOfWork.SpecializationRepository.ListAllAsync(cancellationToken);

            var dtos = mapper.Map<List<SpecializationDto>>(specializations);

            return Result<IReadOnlyList<SpecializationDto>>.Ok(dtos);
        }
    }
}
