using Application.Common.Models;
using Application.Features.Clients.Errors;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.DeleteClient
{
    internal class DeleteClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeleteClientCommand, Result>
    {
        public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellation)
        {
            var client = await unitOfWork.ClientRepository.GetByIdAsync(request.id, cancellation);
            if (client is null)
                return ClientErrors.NotFound;

            await unitOfWork.ClientRepository.DeleteAsync(client, cancellation);
            await unitOfWork.SaveChangesAsync(cancellation);

            return Result.Success();
        }
    }
}
