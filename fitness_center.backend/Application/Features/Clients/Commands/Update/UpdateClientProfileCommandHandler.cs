using Application.Common.Models;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Errors;
using AutoMapper;
using Core.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.Update
{
    internal class UpdateClientProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateClientProfileCommand, Result<ClientDto>>
    {
        public async Task<Result<ClientDto>> Handle(UpdateClientProfileCommand request, CancellationToken cancellationToken)
        {
            Client client = await unitOfWork.ClientRepository.GetByIdAsync(request.Id, cancellationToken);

            if (client == null)
            {
                return ClientErrors.NotFound;
            }

            client.UpdateName(request.Name);
            client.UpdateSurname(request.Surname);


            if (request.ProfilePhotoUrl != null)
            {
                client.SetProfilePhoto(request.ProfilePhotoUrl);
            }

            await unitOfWork.ClientRepository.UpdateAsync(client, cancellationToken);

            var saved = await unitOfWork.SaveChangesAsync(cancellationToken);

            return mapper.Map<ClientDto>(client);
        }
    }
}
