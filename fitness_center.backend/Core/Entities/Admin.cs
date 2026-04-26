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
        public Admin(Guid id, string? name, IContactInfo? contact) :
            base(id, name, contact, null)
        { }
    }
}
