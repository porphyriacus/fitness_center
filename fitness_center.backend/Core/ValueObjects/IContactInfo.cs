using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObjects
{
    public abstract record ContactInfo
    {
        public abstract string Type { get; }
        public abstract string GetContact();

        public abstract string Serialize();

        public static ContactInfo? Deserialize(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            var parts = data.Split('|');
            if (parts.Length != 2) return null;

            var type = parts[0];
            var value = parts[1];

            return type switch
            {
                "Email" => new EmailContactInfo(value),
                "Phone" => new PhoneContactInfo(value),
                _ => null
            };
        }
    }

}
