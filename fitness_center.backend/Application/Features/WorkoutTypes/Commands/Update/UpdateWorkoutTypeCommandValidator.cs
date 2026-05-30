using Application.Features.WorkoutTypes.Commands.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Update
{
    public class UpdateWorkoutTypeCommandValidator : AbstractValidator<UpdateWorkoutTypeCommand>
    {
        public UpdateWorkoutTypeCommandValidator()
        {
            RuleFor(x => x.Description)
                .NotNull()
                .MaximumLength(300).WithMessage("Описание не может быть длиннее 300 символов");
            RuleFor(x => x.Price)
              .GreaterThanOrEqualTo(0).WithMessage("Стоимость тренировки не может быть отрицательной");
        }
    }
}
