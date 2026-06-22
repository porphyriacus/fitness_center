using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository ClientRepository { get; }
        IRepository<Trainer> TrainerRepository { get; }
        IRepository<Specialization> SpecializationRepository { get; }

        IRepository<Booking> BookingRepository { get; }

        IRepository<Membership> MembershipRepository { get; }
        IRepository<MembershipType> MembershipTypeRepository { get; }

        IRepository<Workout> WorkoutRepository { get; }
        IRepository<WorkoutType> WorkoutTypeRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
