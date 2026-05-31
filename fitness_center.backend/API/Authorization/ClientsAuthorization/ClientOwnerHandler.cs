using Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Authorization.ClientsAuthorization
{

    public class ClientOwnerHandler(IUnitOfWork unitOfWork) : AuthorizationHandler<ClientOwnerRequirement, int>
    {
        protected override async Task HandleRequirementAsync( AuthorizationHandlerContext context
                                                              , ClientOwnerRequirement requirement
                                                              , int clientId)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            var identityUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isOwner = await unitOfWork.ClientRepository.FirstOrDefaultAsync(c => c.Id == clientId && c.IdentityUserId == identityUserId);

            if (isOwner != null)
                context.Succeed(requirement);
        }
    }

}
