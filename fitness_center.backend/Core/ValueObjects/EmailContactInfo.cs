using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public sealed class EmailContactInfo : ContactInfo
    {
        private EmailContactInfo() { }

        public EmailContactInfo(string email) : base("Email", email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Некорректный email");
        }
    }
}
