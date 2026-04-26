using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Trainer: Person
    {
        public string? Description {  get; set; }
        public Trainer(Guid id, string? name, IContactInfo? contact, string? profilePhotoUrl, string? description) :
            base(id, name, contact, profilePhotoUrl)
        {
            Description = description?? string.Empty; ;
        }
    }
}
