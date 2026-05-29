using Application.Common.Models;
using Application.Features.Clients.Errors;
using Core.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.Update
{
    internal class UpdateClientProfileCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateClientProfileCommand, Result>
    {
        public async Task<Result> Handle(UpdateClientProfileCommand request, CancellationToken cancellationToken)
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
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
