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

namespace Application.Features.Clients.Commands.Create
{
internal class CreateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
    : IRequestHandler<CreateClientCommand, Result<ClientDto>>
{
    public async Task<Result<ClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = new Client(
            request.Name, 
            request.Surname, 
            request.IdentityUserId, 
            request.ProfilePhotoUrl);

        await unitOfWork.ClientRepository.AddAsync(client, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<ClientDto>(client);
    }
}
}
