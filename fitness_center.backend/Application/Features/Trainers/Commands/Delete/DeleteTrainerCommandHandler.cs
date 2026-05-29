using Application.Common.Models;
using Application.Features.Trainers.Commands.Update;
using Application.Features.Trainers.Errors;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Commands.Delete
{
    internal class DeleteTrainerCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteTrainerCommand, Result>
    {
        public async Task<Result> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
        {
            var trainer = await unitOfWork.TrainerRepository.GetByIdAsync(request.id, cancellationToken);
            if(trainer == null)
            {
                return TrainerErrors.NotFound;
            }

            await unitOfWork.TrainerRepository.DeleteAsync(trainer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
