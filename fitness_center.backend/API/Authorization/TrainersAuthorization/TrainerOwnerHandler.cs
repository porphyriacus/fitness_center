using Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Authorization.TrainersAuthorization
{
    public class TrainerOwnerHandler(IUnitOfWork unitOfWork) : AuthorizationHandler<TrainerOwnerRequirement, int>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context
            , TrainerOwnerRequirement requirement
            , int trainerId)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            var tokenId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var trainer = await unitOfWork.TrainerRepository.FirstOrDefaultAsync(t => t.IdentityUserId == tokenId
                                                                                   && t.Id == trainerId);
            if (trainer != null)
                context.Succeed(requirement);

        } 
    }
}
