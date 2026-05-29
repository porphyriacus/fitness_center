using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public abstract class ContactInfo
    {
        internal ContactInfo() { }

        protected ContactInfo(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; init; } = string.Empty;
        public string Value { get; protected init; } = string.Empty;

        public string GetContact() => Value;

        public static ContactInfo? Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            if (data.Contains("@"))
                return new EmailContactInfo(data);
            else
                return new PhoneContactInfo(data);
        }
    }

}
