using Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Authorization.BookingsCancel
{
    public class BookingOwnerHandler(IUnitOfWork unitOfWork) : AuthorizationHandler<BookingOwnerRequirement, int>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BookingOwnerRequirement requirement, int bookingId)
        {
            var booking = await unitOfWork.BookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                context.Fail();
                return;
            }
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await unitOfWork.ClientRepository.FirstOrDefaultAsync(c =>    c.IdentityUserId == userId
                                                                                    && c.Id == booking.ClientId);

            if (client != null)
                context.Succeed(requirement);

        }
    }
}
