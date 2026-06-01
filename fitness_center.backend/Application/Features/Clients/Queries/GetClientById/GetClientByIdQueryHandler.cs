using Application.Common.Models;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Errors;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Queries.GetClientById
{
    internal class GetClientByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetClientByIdQuery, Result<ClientDto>>
    {
        public async Task<Result<ClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            Client client = await unitOfWork.ClientRepository.GetByIdAsync(request.id, cancellationToken);
            if (client == null)
            {
                return ClientErrors.NotFound;
            }
            Console.WriteLine($"Fresh client from DB: Id={client.Id}, Name={client.Name}, Surname={client.Surname}");
            return mapper.Map<ClientDto>(client);
        }
    }
}
