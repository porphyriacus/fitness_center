using Application.Features.Bookings.Commands.Create;
using Application.Features.Clients.Commands.Create;
using Application.Features.Memberships.Commands.AssignMembershipToClient;
using Application.Features.MembershipTypes.Commands.Create;
using Application.Features.MembershipTypes.Queries.GetMembershipTypesList;
using Application.Features.Specializations.Commands.Create;
using Application.Features.Specializations.Queries.GetSpecializationsList;
using Application.Features.Trainers.Commands.Create;
using Application.Features.Trainers.Queries.GetTrainersList;
using Application.Features.Workouts.Commands.Create;
using Application.Features.Workouts.Queries.GetWorkoutsList;
using Application.Features.WorkoutTypes.Commands.Create;
using Application.Features.WorkoutTypes.Queries.GetWorkoutTypesList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class SeedDataModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public SeedDataModel(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public string? Message { get; set; }
        public string? Error { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var random = new Random();

                // 1. Специализации
                var specializations = new[] { "Йога", "Кроссфит", "Пилатес", "Кардио", "Силовые тренировки" };
                foreach (var spec in specializations)
                    await _mediator.Send(new CreateSpecializationCommand(spec));

                // 2. Типы абонементов
                var membershipTypes = new[]
                {
                    new CreateMembershipTypeCommand("Базовый", "4 занятия в месяц", 1500m, 4, 30, false, null),
                    new CreateMembershipTypeCommand("Стандарт", "8 занятий в месяц", 2500m, 8, 30, true, 7),
                    new CreateMembershipTypeCommand("Премиум", "Безлимит", 4000m, null, 30, true, 14)
                };
                foreach (var mt in membershipTypes)
                    await _mediator.Send(mt);

                // 3. Типы тренировок
                var workoutTypes = new[]
                {
                    new CreateWorkoutTypeCommand("Йога", "Расслабляющая практика", 60, 15, "#E8F5E9", 500m),
                    new CreateWorkoutTypeCommand("Пилатес", "Укрепление корпуса", 60, 12, "#E3F2FD", 500m),
                    new CreateWorkoutTypeCommand("Кроссфит", "Интенсивная тренировка", 60, 10, "#FEE2E2", 700m),
                    new CreateWorkoutTypeCommand("Зумба", "Танцевальная фитнес", 60, 20, "#F3E5F5", 450m),
                    new CreateWorkoutTypeCommand("Стретчинг", "Растяжка и гибкость", 60, 15, "#E8E8E8", 400m),
                    new CreateWorkoutTypeCommand("Бокс", "Ударная техника", 60, 12, "#FFE0E0", 800m)
                };
                foreach (var wt in workoutTypes)
                    await _mediator.Send(wt);

                // 4. Создаём тренера
                var specializationsResult = await _mediator.Send(new GetSpecializationsQuery());
                if (!specializationsResult.IsSuccess || !specializationsResult.Value.Any())
                {
                    Error = "Не удалось создать специализации";
                    return Page();
                }
                var specialization = specializationsResult.Value.First();

                var trainerEmail = "trainer@test.com";
                var trainerUser = await _userManager.FindByEmailAsync(trainerEmail);
                if (trainerUser == null)
                {
                    trainerUser = new IdentityUser { UserName = trainerEmail, Email = trainerEmail };
                    await _userManager.CreateAsync(trainerUser, "Trainer123!");
                    await _userManager.AddToRoleAsync(trainerUser, "Trainer");
                }
                await _mediator.Send(new CreateTrainerCommand("Иван", "Петров", trainerUser.Id, null, specialization.Id, "Опытный тренер", 5));

                // Получаем готовые сущности
                var trainersResult = await _mediator.Send(new GetTrainersListQuery());
                var workoutTypesResult = await _mediator.Send(new GetWorkoutTypesListQuery());

                if (!trainersResult.IsSuccess || !trainersResult.Value.Any())
                {
                    Error = "Не удалось получить тренера";
                    return Page();
                }
                if (!workoutTypesResult.IsSuccess || !workoutTypesResult.Value.Any())
                {
                    Error = "Не удалось получить типы тренировок";
                    return Page();
                }

                var trainer = trainersResult.Value.First();
                var types = workoutTypesResult.Value.ToList();

                var now = DateTime.UtcNow;
                var workoutIds = new List<int>();

                // ========== ПРОШЛЫЕ ТРЕНИРОВКИ (3 дня в прошлое) ==========
                // Используем локальное время для создания, но храним в UTC
                var pastDates = new List<DateTime>
                {
                    // Вчера
                    DateTime.UtcNow.AddDays(-1).Date.AddHours(10),
                    DateTime.UtcNow.AddDays(-1).Date.AddHours(14),
                    DateTime.UtcNow.AddDays(-1).Date.AddHours(18),
                    // Позавчера
                    DateTime.UtcNow.AddDays(-2).Date.AddHours(9),
                    DateTime.UtcNow.AddDays(-2).Date.AddHours(13),
                    DateTime.UtcNow.AddDays(-2).Date.AddHours(17),
                    DateTime.UtcNow.AddDays(-2).Date.AddHours(20),
                    // 3 дня назад
                    DateTime.UtcNow.AddDays(-3).Date.AddHours(11),
                    DateTime.UtcNow.AddDays(-3).Date.AddHours(15),
                    DateTime.UtcNow.AddDays(-3).Date.AddHours(19)
                };

                foreach (var date in pastDates)
                {
                    var workoutType = types[random.Next(types.Count)];
                    var command = new CreateWorkoutCommand(workoutType.Id, trainer.Id, date);
                    var result = await _mediator.Send(command);
                    if (result.IsSuccess)
                        workoutIds.Add(result.Value.Id);
                }

                // ========== СЕГОДНЯШНИЕ И БУДУЩИЕ ТРЕНИРОВКИ (на 7 дней вперёд) ==========
                var timeSlots = new[] { 8, 10, 12, 14, 16, 18, 20 };

                for (int day = 0; day <= 7; day++)
                {
                    var futureDate = DateTime.UtcNow.Date.AddDays(day);
                    int workoutsCount = day == 0 ? random.Next(2, 4) : random.Next(3, 5);
                    var usedHours = new HashSet<int>();

                    for (int i = 0; i < workoutsCount; i++)
                    {
                        var available = timeSlots.Where(h => !usedHours.Contains(h)).ToList();
                        if (!available.Any()) break;
                        var hour = available[random.Next(available.Count)];
                        usedHours.Add(hour);
                        var workoutTime = futureDate.AddHours(hour);

                        // Для сегодня пропускаем уже прошедшее время
                        if (day == 0 && workoutTime <= DateTime.UtcNow) continue;

                        var workoutType = types[random.Next(types.Count)];
                        var command = new CreateWorkoutCommand(workoutType.Id, trainer.Id, workoutTime);
                        var result = await _mediator.Send(command);
                        if (result.IsSuccess)
                            workoutIds.Add(result.Value.Id);
                    }
                }

                // 5. Клиенты (10 штук)
                var clients = new List<(string Name, string Surname, string Email, string Password)>
                {
                    ("Анна", "Смирнова", "anna@test.com", "Client123!"),
                    ("Дмитрий", "Кузнецов", "dmitry@test.com", "Client123!"),
                    ("Елена", "Васильева", "elena@test.com", "Client123!"),
                    ("Сергей", "Михайлов", "sergey@test.com", "Client123!"),
                    ("Ольга", "Федорова", "olga@test.com", "Client123!"),
                    ("Павел", "Соколов", "pavel@test.com", "Client123!"),
                    ("Мария", "Попова", "maria@test.com", "Client123!"),
                    ("Алексей", "Лебедев", "alexey@test.com", "Client123!"),
                    ("Наталья", "Новикова", "natalia@test.com", "Client123!"),
                    ("Владимир", "Козлов", "vladimir@test.com", "Client123!")
                };

                var clientIds = new List<int>();
                foreach (var c in clients)
                {
                    var identity = await _userManager.FindByEmailAsync(c.Email);
                    if (identity == null)
                    {
                        identity = new IdentityUser { UserName = c.Email, Email = c.Email };
                        await _userManager.CreateAsync(identity, c.Password);
                        await _userManager.AddToRoleAsync(identity, "Client");
                    }
                    var clientResult = await _mediator.Send(new CreateClientCommand(c.Name, c.Surname, identity.Id, null));
                    if (clientResult.IsSuccess)
                        clientIds.Add(clientResult.Value.Id);
                }

                // 6. Абонементы для клиентов
                var membershipList = await _mediator.Send(new GetMembershipTypesListQuery());
                var premium = membershipList.Value.First(x => x.Name == "Премиум");
                var standard = membershipList.Value.First(x => x.Name == "Стандарт");
                var basic = membershipList.Value.First(x => x.Name == "Базовый");

                for (int i = 0; i < clientIds.Count; i++)
                {
                    if (i % 3 == 0)
                        await _mediator.Send(new AssignMembershipToClientCommand(clientIds[i], premium.Id));
                    else if (i % 3 == 1)
                        await _mediator.Send(new AssignMembershipToClientCommand(clientIds[i], standard.Id));
                    else
                        await _mediator.Send(new AssignMembershipToClientCommand(clientIds[i], basic.Id));
                }

                // 7. Получаем ВСЕ тренировки
                var allWorkouts = await _mediator.Send(new GetWorkoutsListQuery { IncludePast = true });
                var pastWorkouts = allWorkouts.Value.Where(w => w.StartsAt < DateTime.UtcNow).ToList();
                var futureWorkouts = allWorkouts.Value.Where(w => w.StartsAt >= DateTime.UtcNow).ToList();

                // 8. Заполняем прошлые тренировки (40-80%)
                foreach (var w in pastWorkouts)
                {
                    int capacity = w.DefaultMaxCapacity;
                    int count = random.Next((int)(capacity * 0.4), (int)(capacity * 0.8) + 1);
                    count = Math.Min(count, clientIds.Count);
                    var shuffled = clientIds.OrderBy(x => random.Next()).Take(count).ToList();
                    foreach (var clientId in shuffled)
                        await _mediator.Send(new CreateBookingCommand(clientId, w.Id));
                }

                // 9. 3 будущие тренировки заполняем почти полностью
                foreach (var w in futureWorkouts.Take(3))
                {
                    int capacity = w.DefaultMaxCapacity;
                    int count = (int)(capacity * 0.9);
                    count = Math.Min(count, clientIds.Count);
                    var shuffled = clientIds.OrderBy(x => random.Next()).Take(count).ToList();
                    foreach (var clientId in shuffled)
                        await _mediator.Send(new CreateBookingCommand(clientId, w.Id));
                }

                // 10. Остальные будущие – частично (10-30%)
                foreach (var w in futureWorkouts.Skip(3))
                {
                    int capacity = w.DefaultMaxCapacity;
                    int count = random.Next(1, (int)(capacity * 0.3) + 1);
                    count = Math.Min(count, clientIds.Count);
                    var shuffled = clientIds.OrderBy(x => random.Next()).Take(count).ToList();
                    foreach (var clientId in shuffled)
                        await _mediator.Send(new CreateBookingCommand(clientId, w.Id));
                }

                Message = $"Тренировок: {workoutIds.Count} (прошлых: {pastWorkouts.Count}, будущих: {futureWorkouts.Count})\nКлиентов: {clientIds.Count}\nЗаписей: {allWorkouts.Value.Sum(w => w.CurrentBookingsCount)}";
            }
            catch (Exception ex)
            {
                Error = $"Ошибка: {ex.Message}";
            }

            return Page();
        }
    }
}