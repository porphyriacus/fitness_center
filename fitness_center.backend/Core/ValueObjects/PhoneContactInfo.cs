using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public sealed class PhoneContactInfo : ContactInfo
    {
        private PhoneContactInfo() { }

        public PhoneContactInfo(string phoneNumber) : base("Phone", phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Некорректный телефон");
        }
    }
}
