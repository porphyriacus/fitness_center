using Application.Features.Bookings;
using Application.Features.Bookings.Commands.CancelBooking;
using Application.Features.Bookings.Commands.Create;
using Application.Features.Bookings.Queries;
using Application.Features.Bookings.Queries.GetClientBookingsHistory;
using Application.Features.Clients.Queries.GetClientByUserId;
using Application.Features.Workouts;
using Application.Features.Workouts.Queries.GetWorkoutsList;
using Application.Features.WorkoutTypes;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages.Workouts
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public List<WorkoutDto> Workouts { get; set; } = new();
        public List<WorkoutTypeDto> WorkoutTypes { get; set; } = new();
        public List<BookingDto> ClientBookings { get; set; } = new();

        public async Task OnGetAsync()
        {
            var typesResult = await _mediator.Send(new GetWorkoutTypesListQuery());
            if (typesResult.IsSuccess)
                WorkoutTypes = typesResult.Value.ToList();

            var query = new GetWorkoutsListQuery
            {
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(14),
                IncludePast = true
            };

            var result = await _mediator.Send(query);
            if (result.IsSuccess)
                Workouts = result.Value.ToList();

            // Загружаем бронирования текущего клиента
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var clientResult = await _mediator.Send(new GetClientByUserIdQuery(user.Id));
                if (clientResult.IsSuccess && clientResult.Value != null)
                {
                    var bookingsResult = await _mediator.Send(new GetClientBookingsHistoryQuery(clientResult.Value.Id));
                    if (bookingsResult.IsSuccess)
                        ClientBookings = bookingsResult.Value.ToList();
                }
            }
        }

        public async Task<IActionResult> OnPostBookAsync(int workoutId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "Необходимо войти в систему" }) { StatusCode = 401 };
            }

            var clientResult = await _mediator.Send(new GetClientByUserIdQuery(user.Id));
            if (!clientResult.IsSuccess || clientResult.Value == null)
            {
                return new JsonResult(new { success = false, message = "Профиль клиента не найден" }) { StatusCode = 400 };
            }

            var command = new CreateBookingCommand(clientResult.Value.Id, workoutId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return new JsonResult(new { success = false, message = result.Error.Message }) { StatusCode = 400 };
            }

            return new JsonResult(new { success = true, message = "Вы успешно записаны на тренировку" });
        }

        public async Task<IActionResult> OnPostCancelAsync(int workoutId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "Необходимо войти в систему" }) { StatusCode = 401 };
            }

            var clientResult = await _mediator.Send(new GetClientByUserIdQuery(user.Id));
            if (!clientResult.IsSuccess || clientResult.Value == null)
            {
                return new JsonResult(new { success = false, message = "Профиль клиента не найден" }) { StatusCode = 400 };
            }

            var bookingsResult = await _mediator.Send(new GetClientBookingsQuery(clientResult.Value.Id));
            if (bookingsResult.IsSuccess)
            {
                var booking = bookingsResult.Value.FirstOrDefault(b => b.WorkoutId == workoutId && b.Status == "Active");
                if (booking != null)
                {
                    var command = new CancelBookingCommand(booking.Id);
                    var result = await _mediator.Send(command);
                    if (!result.IsSuccess)
                    {
                        return new JsonResult(new { success = false, message = result.Error.Message }) { StatusCode = 400 };
                    }
                }
            }

            return new JsonResult(new { success = true, message = "Запись успешно отменена" });
        }
    }
}