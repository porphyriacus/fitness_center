using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MembershipTypes.Commands.Create
{
    public class CreateMembershipTypeCommandValidator : AbstractValidator<CreateMembershipTypeCommand>
    {
        public CreateMembershipTypeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название не может быть пустым")
                .MaximumLength(100).WithMessage("Название не может быть длиннее 100 символов");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Цена не может быть отрицательной");

            RuleFor(x => x.ValidityDays)
                .GreaterThan(0).WithMessage("Срок действия должен быть положительным");

            RuleFor(x => x.SessionsCount)
                .GreaterThan(0).When(x => x.SessionsCount.HasValue)
                .WithMessage("Количество занятий должно быть положительным");

            RuleFor(x => x.MaxFreezeDays)
                .GreaterThan(0).When(x => x.MaxFreezeDays.HasValue)
                .WithMessage("Длительность заморозки должна быть положительной");

            When(x => x.CanFreeze, () =>
            {
                RuleFor(x => x.MaxFreezeDays)
                    .NotNull().WithMessage("При возможности заморозки укажите максимальную длительность");
            });

            When(x => !x.CanFreeze, () =>
            {
                RuleFor(x => x.MaxFreezeDays)
                    .Null().WithMessage("Если заморозка недоступна, MaxFreezeDays должен быть null");
            });
        }
    }
}
