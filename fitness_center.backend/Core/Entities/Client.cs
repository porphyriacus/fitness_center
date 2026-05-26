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

        public DateTime RegisteredAt { get; private set; }

        private Client() { }

        public Client(string name, ContactInfo contact, string identityUserId)
            : base(name, contact, identityUserId)
        {
            RegisteredAt = DateTime.UtcNow;
        }

        public bool HasActiveMembership()
        {
            return Membership != null && !Membership.IsFinished;
        }
    }
}
