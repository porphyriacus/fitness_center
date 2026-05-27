using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Clients.Commands.Update
{
    public class UpdateClientProfileCommandValidator : AbstractValidator<UpdateClientProfileCommand>
    {
        public UpdateClientProfileCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Имя не должно быть пустым");
            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Имя не должно быть пустым");

            RuleFor(x => x.Contact)
                .NotEmpty().WithMessage("Контакт обязателен")
                .Must(BeValidContact).WithMessage("Некорректный формат контакта (email или телефон)");

            RuleFor(x => x.ProfilePhotoUrl)
                .MaximumLength(500).WithMessage("URL фото не может быть длиннее 500 символов");
        }

        private bool BeValidContact(string contact)
        {
            return IsValidEmail(contact) || IsValidPhone(contact);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?[0-9]{10,15}$");
        }
    }
}
