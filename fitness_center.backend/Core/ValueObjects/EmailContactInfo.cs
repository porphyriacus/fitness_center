using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public class EmailContactInfo : IContactInfo
    {
        public string Email { get; }
        public string Type => "Email";

        public EmailContactInfo(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Некорректный email", nameof(email));
            Email = email;
        }

        public string GetContact() => Email;
    }
}
