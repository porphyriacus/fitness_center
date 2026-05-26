using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public sealed record PhoneContactInfo : ContactInfo
    {
        public string PhoneNumber { get; }
        public override string Type => "Phone";

        public PhoneContactInfo(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым", nameof(phoneNumber));
            PhoneNumber = phoneNumber;
        }

        public override string GetContact() => PhoneNumber;
        public override string Serialize() => $"Phone|{PhoneNumber}";
    }
}
