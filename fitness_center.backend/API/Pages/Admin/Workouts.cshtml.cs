using Application.Features.Bookings;
using Application.Features.Bookings.Commands.Create;
using Application.Features.Bookings.Commands.MarkAttendance;
using Application.Features.Bookings.Commands.MarkNoShow;
using Application.Features.Bookings.Queries.GetBookingsByWorkout;
using Application.Features.Clients.DTOs;
using Application.Features.Clients.Queries.GetClientsList;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Queries.GetTrainersList;
using Application.Features.Workouts;
using Application.Features.Workouts.Commands.Cancel;
using Application.Features.Workouts.Commands.Create;
using Application.Features.Workouts.Commands.Delete;
using Application.Features.Workouts.Commands.Update;
using Application.Features.Workouts.Queries.GetWorkoutById;
using Application.Features.Workouts.Queries.GetWorkoutsList;
using Application.Features.WorkoutTypes;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class WorkoutsModel : PageModel
    {
        private readonly IMediator _mediator;

        public WorkoutsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<WorkoutDto> Workouts { get; set; } = new();
        public List<WorkoutTypeDto> WorkoutTypes { get; set; } = new();
        public List<TrainerDto> Trainers { get; set; } = new();
        public List<ClientDto> Clients { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var typesResult = await _mediator.Send(new GetWorkoutTypesListQuery());
            if (typesResult.IsSuccess) WorkoutTypes = typesResult.Value.ToList();

            var trainersResult = await _mediator.Send(new GetTrainersListQuery());
            if (trainersResult.IsSuccess) Trainers = trainersResult.Value.ToList();

            var workoutsResult = await _mediator.Send(new GetWorkoutsListQuery { IncludePast = true });
            if (workoutsResult.IsSuccess) Workouts = workoutsResult.Value.OrderBy(w => w.StartsAt).ToList();

            var clientsResult = await _mediator.Send(new GetClientsListQuery());
            if (clientsResult.IsSuccess) Clients = clientsResult.Value.ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync(int workoutTypeId, int trainerId, DateTime startAt)
        {
            // Проверка времени: не раньше чем через 8 часов
            if (startAt < DateTime.UtcNow.AddHours(8))
            {
                TempData["CreateError"] = "Тренировку можно создать минимум за 8 часов до начала";
                await LoadData();
                return Page();
            }

            if (startAt.Hour < 8 || startAt.Hour >= 22)
            {
                TempData["CreateError"] = "Время тренировки должно быть с 8:00 до 22:00";
                await LoadData();
                return Page();
            }

            if (startAt > DateTime.UtcNow.AddDays(14))
            {
                TempData["CreateError"] = "Можно создавать тренировки только на ближайшие 14 дней";
                await LoadData();
                return Page();
            }

            var command = new CreateWorkoutCommand(workoutTypeId, trainerId, startAt);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["CreateError"] = result.Error.Message;
                await LoadData();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int workoutId, int trainerId, DateTime startAt)
        {
            // Проверка времени: нельзя переносить на время раньше чем через 8 часов
            if (startAt < DateTime.UtcNow.AddHours(8))
            {
                TempData["EditError"] = "Нельзя переносить тренировку на время раньше чем через 8 часов";
                await LoadData();
                return Page();
            }

            if (startAt.Hour < 8 || startAt.Hour >= 22)
            {
                TempData["EditError"] = "Время тренировки должно быть с 8:00 до 22:00";
                await LoadData();
                return Page();
            }

            var command = new UpdateWorkoutCommand(workoutId, trainerId, startAt);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["EditError"] = result.Error.Message;
                await LoadData();
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var command = new CancelWorkoutCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["EditError"] = result.Error.Message;
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAllAsync()
        {
            var allWorkouts = await _mediator.Send(new GetWorkoutsListQuery { IncludePast = true });
            if (allWorkouts.IsSuccess)
            {
                foreach (var workout in allWorkouts.Value)
                {
                    await _mediator.Send(new DeleteWorkoutCommand(workout.Id));
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetBookings(int workoutId)
        {
            var query = new GetBookingsByWorkoutQuery(workoutId);
            var result = await _mediator.Send(query);

            if (result.IsSuccess)
            {
                return new JsonResult(result.Value);
            }

            return new JsonResult(new { error = "Ошибка загрузки" }) { StatusCode = 500 };
        }

        public async Task<IActionResult> OnPostMarkAttendance(int bookingId)
        {
            var command = new MarkAttendanceCommand(bookingId);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { error = result.Error.Message }) { StatusCode = 400 };
        }

        public async Task<IActionResult> OnPostMarkNoShow(int bookingId)
        {
            var command = new MarkNoShowCommand(bookingId);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { error = result.Error.Message }) { StatusCode = 400 };
        }


        public async Task<IActionResult> OnPostAddBooking(int workoutId, int clientId)
        {
            // Проверяем свободные места
            var workout = await _mediator.Send(new GetWorkoutByIdQuery(workoutId));
            if (!workout.IsSuccess)
            {
                return new JsonResult(new { error = "Тренировка не найдена" }) { StatusCode = 404 };
            }

            if (workout.Value.AvailableSlots <= 0)
            {
                return new JsonResult(new { error = "Нет свободных мест" }) { StatusCode = 409 };
            }

            var command = new CreateBookingCommand(clientId, workoutId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return new JsonResult(new { error = result.Error.Message }) { StatusCode = 400 };
            }

            // После успешной записи получаем обновлённую тренировку
            var updatedWorkout = await _mediator.Send(new GetWorkoutByIdQuery(workoutId));

            return new JsonResult(new
            {
                success = true,
                updatedWorkout = updatedWorkout.IsSuccess ? updatedWorkout.Value : null
            });
        }
    }
}