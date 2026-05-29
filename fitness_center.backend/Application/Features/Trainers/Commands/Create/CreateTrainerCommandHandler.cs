using Application.Common.Models;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Errors;
using AutoMapper;
using Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Commands.Create
{
    internal class CreateTrainerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateTrainerCommand, Result<TrainerDto>>
    {
        public async Task<Result<TrainerDto>> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
        {
            Specialization specialization = await unitOfWork.SpecializationRepository.GetByIdAsync(request.SpecializationId, cancellationToken);
            if (specialization == null)
                return TrainerErrors.SpecializationNotFound;

            Trainer trainer = new Trainer(
                  request.Name
                , request.Surname
                , request.IdentityUserId
                , request.ProfilePhotoUrl
                , specialization
                , request.Description
                , request.ExperienceYears
            );

            await unitOfWork.TrainerRepository.AddAsync(trainer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<TrainerDto>(trainer);

        }
    }
}
