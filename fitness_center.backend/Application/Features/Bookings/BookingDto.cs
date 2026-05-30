using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; } = string.Empty;
        public DateTime StartsAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime BookedAt { get; set; }
    }
}
