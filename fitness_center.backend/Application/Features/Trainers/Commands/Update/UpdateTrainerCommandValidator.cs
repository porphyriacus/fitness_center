using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Trainers.Commands.Update
{
    public class UpdateTrainerCommandValidator : AbstractValidator<UpdateTrainerCommand>
    {
        public UpdateTrainerCommandValidator()
        {
            RuleFor(x => x.Name)
               .NotNull().WithMessage("Имя не должно быть пустым");
            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Фамилия не должна быть пуста");
            RuleFor(x => x.SpecializationId)
                .GreaterThan(0).WithMessage("Специализация обязательна");
            RuleFor(x => x.ExperienceYears)
               .GreaterThanOrEqualTo(0).WithMessage("Опыт работы не может быть отрицательным");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Описание не может быть длиннее 500 символов");
            RuleFor(x => x.ProfilePhotoUrl)
                .MaximumLength(500).WithMessage("URL фото не может быть длиннее 500 символов");
        }
    }
}
