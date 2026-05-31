using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Update
{
    public class UpdateMembershipTypeCommandValidator : AbstractValidator<UpdateMembershipTypeCommand>
    {
        public UpdateMembershipTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id должен быть больше 0");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Название не может быть пустым")
                .MaximumLength(100)
                .WithMessage("Название не может быть длиннее 100 символов");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Описание не может быть длиннее 500 символов");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Цена не может быть отрицательной");
        }
    }
}
