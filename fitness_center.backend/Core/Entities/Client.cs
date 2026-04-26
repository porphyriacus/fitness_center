using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Client: Person
    {
        public Client(Guid id, string? name, IContactInfo? contact, string? profilePhotoUrl) :
            base(id, name, contact, profilePhotoUrl){ }
        
    }
}
