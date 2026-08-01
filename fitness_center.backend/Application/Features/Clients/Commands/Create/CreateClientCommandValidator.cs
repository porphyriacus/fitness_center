using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.Create
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Имя не должно быть пустым");
            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Фамилия не должна быть пуста");
            RuleFor(x => x.ProfilePhotoUrl)
                .MaximumLength(500).WithMessage("URL фото не может быть длиннее 500 символов");
        }
    }
}
