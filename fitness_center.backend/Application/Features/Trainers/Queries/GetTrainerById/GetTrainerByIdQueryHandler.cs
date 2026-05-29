using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Errors;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Queries.GetTrainerById
{
    internal class GetTrainerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetTrainerByIdQuery, Result<TrainerDto>>
    {
        public async Task<Result<TrainerDto>> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
        {
            var tr = await unitOfWork.TrainerRepository.GetByIdAsync(request.id);
            if (tr == null)
            {
                return TrainerErrors.NotFound;
            }

            return mapper.Map<TrainerDto>(tr);
        }
    }
}
