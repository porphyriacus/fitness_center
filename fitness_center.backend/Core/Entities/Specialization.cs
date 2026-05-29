using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Specialization : Entity
    {
        public string Name { get; private set; }

        private Specialization() { }

        public Specialization(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название специализации не может быть пустым", nameof(name));
            Name = name;

        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Название не может быть пустым", nameof(newName));
            Name = newName;
        }
    }
}
