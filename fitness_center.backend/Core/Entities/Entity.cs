using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public abstract class Entity
    {
        public int Id { get; private set; }  
        protected Entity() { }
        protected Entity(int id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;


            if (Id == 0 || other.Id == 0)
                return false;

            return Id == other.Id;
        }


        public override int GetHashCode()
        {
            if (Id == 0)
                return GetType().GetHashCode();

            return Id.GetHashCode();
        }
    }

}
