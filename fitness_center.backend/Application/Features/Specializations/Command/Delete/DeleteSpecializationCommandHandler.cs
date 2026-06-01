using Application.Common.Models;
using Application.Features.Specializations.Commands;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations.Commands.DeleteSpecialization
{
    internal class DeleteSpecializationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteSpecializationCommand, Result>
    {
        public async Task<Result> Handle(DeleteSpecializationCommand request, CancellationToken cancellation)
        {
            var specialization = await unitOfWork.SpecializationRepository.GetByIdAsync(request.id, cancellation);
            if (specialization is null)
                return SpecializationErrors.NotFound;

            await unitOfWork.SpecializationRepository.DeleteAsync(specialization, cancellation);
            await unitOfWork.SaveChangesAsync(cancellation);

            return Result.Success();
        }
    }
}
