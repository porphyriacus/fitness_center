using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Update
{
    public class UpdateWorkoutTypeCommandValidator : AbstractValidator<UpdateWorkoutTypeCommand>
    {
        public UpdateWorkoutTypeCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Описание не может быть длиннее 500 символов");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Цена не может быть отрицательной");

            RuleFor(x => x.Color)
                .Must(color => color == null || !string.IsNullOrWhiteSpace(color))
                .WithMessage("Цвет не может быть пустой строкой");
        }
    }
}
