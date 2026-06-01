using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Specializations.Commands.Create;

public class CreateSpecializationValidator : AbstractValidator<CreateSpecializationCommand>
{
    public CreateSpecializationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название специализации обязательно")
            .MaximumLength(100).WithMessage("Название не может превышать 100 символов")
            .MinimumLength(2).WithMessage("Название должно содержать минимум 2 символа");
    }
}