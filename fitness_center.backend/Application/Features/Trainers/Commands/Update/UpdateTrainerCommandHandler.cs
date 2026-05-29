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

namespace Application.Features.Trainers.Commands.Update
{
    internal class UpdateTrainerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateTrainerCommand, Result<TrainerDto>>
    {
        public async Task<Result<TrainerDto>> Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
        {
            Trainer trainer = await unitOfWork.TrainerRepository.GetByIdAsync(request.Id);
            if (trainer == null) { 
                return TrainerErrors.NotFound;
            }

            trainer.UpdateName(request.Name);
            trainer.UpdateSurname(request.Surname);

            trainer.UpdateDescription(request.Description);

            if (request.ProfilePhotoUrl != null)
            {
                trainer.SetProfilePhoto(request.ProfilePhotoUrl);
            }
            
            trainer.UpdateExperience(request.ExperienceYears);

            Specialization specialization = await unitOfWork.SpecializationRepository.GetByIdAsync(request.SpecializationId, cancellationToken);
            if (specialization == null)
                return TrainerErrors.SpecializationNotFound;

            trainer.UpdateSpecialization(specialization);

            await unitOfWork.TrainerRepository.UpdateAsync(trainer, cancellationToken);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<TrainerDto>(trainer);

        }
    }
}
