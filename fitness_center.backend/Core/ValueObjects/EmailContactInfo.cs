using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public sealed record EmailContactInfo : ContactInfo
    {
        public string Email { get; }
        public override string Type => "Email";

        public EmailContactInfo(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                throw new ArgumentException("Некорректный email", nameof(email));
            Email = email;
        }

        public override string GetContact() => Email;
    }
}
