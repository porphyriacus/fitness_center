using Application.Common.Models;
using Application.Features.Clients.Commands.Create;
using Application.Features.Clients.DTOs;
using Application.Features.Specializations.Commands.Create;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Specializations.Command.Create
{
    internal class CreateSpecializationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            : IRequestHandler<CreateSpecializationCommand, Result<SpecializationDto>>
    {
        public async Task<Result<SpecializationDto>> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
        {
            var client = new Specialization(
                request.Name
            );

            await unitOfWork.SpecializationRepository.AddAsync(client, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<SpecializationDto>(client);
        }
    }
}
