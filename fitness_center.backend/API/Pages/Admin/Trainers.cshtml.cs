using Application.Common.Models;
using Application.Features.Specializations;
using Application.Features.Specializations.Queries.GetSpecializationsList;
using Application.Features.Trainers.Commands.Create;
using Application.Features.Trainers.Commands.Delete;
using Application.Features.Trainers.Commands.Update;
using Application.Features.Trainers.DTOs;
using Application.Features.Trainers.Queries.GetTrainersList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class TrainersModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public TrainersModel(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public List<TrainerDto> Trainers { get; set; } = new();
        public List<SpecializationDto> Specializations { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var trainersResult = await _mediator.Send(new GetTrainersListQuery());
            if (trainersResult.IsSuccess)
                Trainers = trainersResult.Value.ToList();

            var specsResult = await _mediator.Send(new GetSpecializationsQuery());
            if (specsResult.IsSuccess)
                Specializations = specsResult.Value.ToList();
        }

        public async Task<IActionResult> OnPostCreateAsync(string name, string surname, string email, string password, int specializationId, int experienceYears, string? description)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                TempData["CreateError"] = "Пароль должен быть не менее 6 символов";
                await LoadData();
                return Page();  
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                TempData["CreateError"] = "Пользователь с таким email уже существует";
                await LoadData();
                return Page(); 
            }

            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                TempData["CreateError"] = string.Join(", ", result.Errors.Select(e => e.Description));
                await LoadData();
                return Page();  
            }

            await _userManager.AddToRoleAsync(user, "Trainer");

            var command = new CreateTrainerCommand(name, surname, user.Id, null, specializationId, description, experienceYears);
            var sendResult = await _mediator.Send(command);

            if (!sendResult.IsSuccess)
            {
                TempData["CreateError"] = sendResult.Error.Message;
                await LoadData();
                return Page();  
            }

            await LoadData();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id, string name, string surname, int specializationId, int experienceYears, string? description)
        {
            var command = new UpdateTrainerCommand(id, name, surname, null, specializationId, description, experienceYears);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                TempData["EditError"] = result.Error.Message;
                await LoadData();
                return Page();
            }

            await LoadData();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _mediator.Send(new DeleteTrainerCommand(id));
            await LoadData();
            return RedirectToPage();
        }
    }
}