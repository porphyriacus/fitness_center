using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Queries.GetClientByUserId;
using Application.Features.Clients.Commands.Update;
using Application.Features.Memberships.Queries.GetClientMembership;
using Application.Features.Memberships;
using Application.Features.Bookings;
using Application.Features.Bookings.Queries;
using Application.Features.Bookings.Queries.GetClientBookingsHistory;
using Application.Features.Bookings.Commands.CancelBooking;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Queries.GetTrainerByUserId;
using Application.Features.Trainers.Queries.GetTrainerSchedule;
using Application.Features.Workouts;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using Application.Features.WorkoutTypes;

namespace API.Pages.Account.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public ClientDto? Client { get; set; }
        public TrainerDto? Trainer { get; set; }
        public MembershipDto? Membership { get; set; }
        public List<BookingDto> CurrentBookings { get; set; } = new();
        public List<BookingDto> HistoryBookings { get; set; } = new();
        public List<WorkoutDto> Workouts { get; set; } = new();
        public List<WorkoutTypeDto> WorkoutTypes { get; set; } = new();
        public string UserRole { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            var typesResult = await _mediator.Send(new GetWorkoutTypesListQuery());
            if (typesResult.IsSuccess)
                WorkoutTypes = typesResult.Value.ToList();

            if (await _userManager.IsInRoleAsync(user, "Client"))
            {
                UserRole = "Client";
                var clientResult = await _mediator.Send(new GetClientByUserIdQuery(user.Id));
                if (clientResult.IsSuccess && clientResult.Value != null)
                {
                    Client = clientResult.Value;
                    var membershipResult = await _mediator.Send(new GetClientMembershipQuery(Client.Id));
                    if (membershipResult.IsSuccess) Membership = membershipResult.Value;
                    var currentBookingsResult = await _mediator.Send(new GetClientBookingsQuery(Client.Id));
                    if (currentBookingsResult.IsSuccess) CurrentBookings = currentBookingsResult.Value.ToList();
                    var historyBookingsResult = await _mediator.Send(new GetClientBookingsHistoryQuery(Client.Id));
                    if (historyBookingsResult.IsSuccess) HistoryBookings = historyBookingsResult.Value.ToList();
                }
            }
            else if (await _userManager.IsInRoleAsync(user, "Trainer"))
            {
                UserRole = "Trainer";
                var trainerResult = await _mediator.Send(new GetTrainerByUserIdQuery(user.Id));
                if (trainerResult.IsSuccess && trainerResult.Value != null)
                {
                    Trainer = trainerResult.Value;
                    var scheduleResult = await _mediator.Send(new GetTrainerScheduleQuery
                    {
                        TrainerId = trainerResult.Value.Id,
                        FromDate = DateTime.UtcNow,
                        ToDate = DateTime.UtcNow.AddDays(14),
                        IncludePast = false
                    });
                    if (scheduleResult.IsSuccess) Workouts = scheduleResult.Value.ToList();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync(int id, string name, string surname, string? profilePhotoUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            if (UserRole == "Client")
            {
                var clientResult = await _mediator.Send(new GetClientByUserIdQuery(user.Id));
                if (!clientResult.IsSuccess || clientResult.Value?.Id != id)
                {
                    TempData["ProfileError"] = "Доступ запрещён";
                    return RedirectToPage();
                }
                var command = new UpdateClientProfileCommand(id, name, surname, null);
                var result = await _mediator.Send(command);
                if (!result.IsSuccess) TempData["ProfileError"] = result.Error.Message;
                else TempData["ProfileSuccess"] = "Профиль обновлён";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelBookingAsync(int bookingId)
        {
            var command = new CancelBookingCommand(bookingId);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess) return new JsonResult(new { error = result.Error.Message }) { StatusCode = 400 };
            return new JsonResult(new { success = true });
        }
    }
}