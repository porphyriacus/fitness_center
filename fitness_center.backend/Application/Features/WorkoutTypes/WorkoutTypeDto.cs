using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.WorkoutTypes
{
    public class WorkoutTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DefaultDurationMinutes { get; set; }
        public int DefaultMaxCapacity { get; set; }
        public string? Color { get; set; } = null;

        public int Price { get; set; }
    }
}
