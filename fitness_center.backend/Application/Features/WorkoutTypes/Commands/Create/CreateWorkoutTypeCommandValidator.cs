using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes.Commands.Create
{
    public class CreateWorkoutTypeCommandValidator : AbstractValidator<CreateWorkoutTypeCommand>
    {
        public CreateWorkoutTypeCommandValidator() { 
            RuleFor(x => x.Name)
               .NotNull().WithMessage("Название не должно быть пустым");
            RuleFor(x => x.Description)
                .NotNull()
                .MaximumLength(300).WithMessage("Описание не может быть длиннее 300 символов");
        }
    }
}
