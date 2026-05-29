using Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Client : Person
    {
        private ICollection<Booking> _bookings = new List<Booking>();
        private Membership? _membership;

        public DateTime RegisteredAt { get; private set; }

        public ICollection<Booking> Bookings
        {
            get => _bookings;
            private set => _bookings = value ?? new List<Booking>();
        }

        public Membership? Membership
        {
            get => _membership;
            private set => _membership = value;
        }

        private Client() { }

        public Client(string name, string surname, string identityUserId, string? profilePhotoUrl)
            : base(name, surname, identityUserId, profilePhotoUrl)
        {
            RegisteredAt = DateTime.UtcNow;
        }

        public bool HasActiveMembership()
        {
            return Membership != null && !Membership.IsFinished;
        }
    }
}
