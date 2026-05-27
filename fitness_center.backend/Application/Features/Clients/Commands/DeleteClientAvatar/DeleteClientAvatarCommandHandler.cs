using Application.Common.Models;
using Application.Features.Clients.Errors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.DeleteClientAvatar
{
    internal class DeleteClientAvatarCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteClientAvatarCommand, Result>
    {
        public async Task<Result> Handle(DeleteClientAvatarCommand request, CancellationToken cancellationToken)
        {
            Client user = await unitOfWork.ClientRepository.GetByIdAsync(request.id, cancellationToken);
            if (user == null) {
                return ClientErrors.NotFound;
            }

            user.RemoveProfilePhoto();
            
            await unitOfWork.ClientRepository.UpdateAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
