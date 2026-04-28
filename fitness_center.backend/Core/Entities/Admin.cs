using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Admin: Person
    {
        public Admin(string? name, IContactInfo? contact) :
            base(name, contact, null)
        { }
    }
}
