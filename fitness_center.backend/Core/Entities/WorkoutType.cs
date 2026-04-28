using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{   
    public class WorkoutType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int DefaultDurationMinutes {  get; private set; }
        public int DefaultMaxCapacity { get; private set; }
        public string? Color { get; private set; } = null;

        public WorkoutType(string? name, string? description, int defaultDurationMinutes, int defaultMaxCapacity, string? color)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым", nameof(name));
            if (defaultDurationMinutes <= 0)
                throw new ArgumentException("Длительность должна быть положительной", nameof(defaultDurationMinutes));
            if (defaultMaxCapacity <= 0)
                throw new ArgumentException("Вместимость должна быть положительной", nameof(defaultMaxCapacity));

            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            DefaultDurationMinutes = defaultDurationMinutes > 0 ? defaultDurationMinutes : 60;
            DefaultMaxCapacity = defaultMaxCapacity;
            Color = color;

        }

        public void ChangeDescription(string? description) {
            if (description?.Length > 160)
                throw new ArgumentException("Описание не длиннее 255 символов");
            Description = description;
        }
        public void ChangeColor(string? color)
        {
            if (color == string.Empty)
                throw new Exception("Некорректный формат цвета");
            Color = color;
        }

    }
}
