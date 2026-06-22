using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    //public interface IMembershipRepository : IRepository<Membership>
    //{
        
    //}
    public class EfMembershipRepository : IRepository<Membership>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Membership> _entities;


        public EfMembershipRepository(AppDbContext context)
        {
            _context = context;
            _entities = _context.Set<Membership>();
        }

        public async Task<Membership?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default,
            params Expression<Func<Membership, object>>[]? includesProperties)
        {
            IQueryable<Membership> query = _entities
                .Include(m => m.MembershipType)  // подгружаем тип для стратегий
                .AsQueryable();

            if (includesProperties != null && includesProperties.Any())
            {
                foreach (var include in includesProperties)
                {
                    query = query.Include(include);
                }
            }

            var membership = await query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            // стратегии после загрузки
            if (membership != null)
            {
                membership.RestoreStrategies(membership.MembershipType);
            }

            return membership;
        }

        public async Task<IReadOnlyList<Membership>> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            var memberships = await _entities
                .Include(m => m.MembershipType)  // подгружаем тип
                .ToListAsync(cancellationToken);

            // восстанавливаем стратегии для всех
            foreach (var membership in memberships)
            {
                membership.RestoreStrategies(membership.MembershipType);
            }

            return memberships;
        }

        public async Task<IReadOnlyList<Membership>> ListAsync(
            Func<IQueryable<Membership>, IOrderedQueryable<Membership>>? orderBy,
            List<Expression<Func<Membership, bool>>>? filters,
            CancellationToken cancellationToken = default,
            params Expression<Func<Membership, object>>[]? includesProperties)
        {
            IQueryable<Membership> query = _entities
                .Include(m => m.MembershipType)  
                .AsQueryable();
            foreach(var filter in filters)
            {
                if (filter != null)
                    query = query.Where(filter);
            }


            if (includesProperties != null && includesProperties.Any())
            {
                foreach (var include in includesProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
                query = orderBy(query);
            else
                query = query.OrderBy(m => m.Id);

            var memberships = await query.ToListAsync(cancellationToken);

            // восстанавливаем стратегии для всех
            foreach (var membership in memberships)
            {
                membership.RestoreStrategies(membership.MembershipType);
            }

            return memberships;
        }

        public async Task<Membership?> FirstOrDefaultAsync(
            Expression<Func<Membership, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            var membership = await _entities
                .Include(m => m.MembershipType)  // подгружаем тип
                .FirstOrDefaultAsync(filter, cancellationToken);

            // восстанавливаем стратегии
            if (membership != null)
            {
                membership.RestoreStrategies(membership.MembershipType);
            }

            return membership;
        }
        
        public async Task AddAsync(
            Membership entity,
            CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(
            Membership entity,
            CancellationToken cancellationToken = default)
        {
            var existing = await _entities.FindAsync(entity.Id, cancellationToken);
            if (existing == null)
            {
                throw new InvalidOperationException($"Entity Membership with id {entity.Id} not found");
            }
            _context.Entry(existing).CurrentValues.SetValues(entity);
        }

        public Task DeleteAsync(
            Membership entity,
            CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            return Task.CompletedTask;
        }
    }
}
