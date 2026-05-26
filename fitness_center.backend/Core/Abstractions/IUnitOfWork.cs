using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Client> Clients { get; }
        IRepository<Trainer> Trainers { get; }

        IRepository<Booking> Bookings { get; }

        IRepository<Membership> Memberships { get; }
        IRepository<MembershipType> MembershipTypes { get; }

        IRepository<Workout> Workouts { get; }
        IRepository<WorkoutType> WorkoutTypes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
