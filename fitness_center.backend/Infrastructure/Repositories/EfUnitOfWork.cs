using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;


        private readonly Lazy<IRepository<Client>> _clientrepository;
        private readonly Lazy<IRepository<Trainer>> _trainerRepository;
        private readonly Lazy<IRepository<Specialization>> _specializationRepository;

        private readonly Lazy<IRepository<Booking>> _bookingRepository;

        private readonly Lazy<IRepository<Membership>> _membershipRepository;
        private readonly Lazy<IRepository<MembershipType>> _membershipTypeRepository;
        private readonly Lazy<IRepository<Workout>> _workoutRepository;
        private readonly Lazy<IRepository<WorkoutType>> _workoutTypeRepository;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;

            _clientrepository = new Lazy<IRepository<Client>>(() =>
                new EfRepository<Client>(context));
            _trainerRepository = new Lazy<IRepository<Trainer>>(() =>
                new EfRepository<Trainer>(context));
            _specializationRepository = new Lazy<IRepository<Specialization>>(()=>
                new EfRepository<Specialization>(context));

            _bookingRepository = new Lazy<IRepository<Booking>>(() =>
                new EfRepository<Booking>(context));


            _membershipRepository = new Lazy<IRepository<Membership>>(() =>
                new EfRepository<Membership>(context));
            _membershipTypeRepository = new Lazy<IRepository<MembershipType>>(() =>
                new EfRepository<MembershipType>(context));

            _workoutRepository = new Lazy<IRepository<Workout>>(() =>
                new EfRepository<Workout>(context));
            _workoutTypeRepository = new Lazy<IRepository<WorkoutType>>(() =>
                new EfRepository<WorkoutType>(context));

        }

        public IRepository<Client> ClientRepository => _clientrepository.Value;
        public IRepository<Trainer> TrainerRepository => _trainerRepository.Value;
        public IRepository<Specialization> SpecializationRepository => _specializationRepository.Value;

        public IRepository<Booking> BookingRepository => _bookingRepository.Value;

        public IRepository<Membership> MembershipRepository => _membershipRepository.Value;
        public IRepository<MembershipType> MembershipTypeRepository => _membershipTypeRepository.Value;

        public IRepository<Workout> WorkoutRepository => _workoutRepository.Value;
        public IRepository<WorkoutType> WorkoutTypeRepository => _workoutTypeRepository.Value;



        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("Транзакция не была начата");

            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("Транзакция не была начата");

            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

    }
}
