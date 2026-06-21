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
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly DbSet<T> _entities;
        protected readonly AppDbContext _context;

        public EfRepository(AppDbContext context){
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id,
         CancellationToken cancellationToken = default,
         params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T> query = _entities.AsQueryable();

            if (includesProperties != null && includesProperties.Any())
            {
                foreach(Expression<Func<T,object>>? include in includesProperties)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }


        public async Task<IReadOnlyList<T>> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _entities.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// TODO сделать список фильтров пока костыльный etQueryable() неприятно   нужно только для тренировок по сути
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="includesProperties"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<T>> ListAsync(
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
            List<Expression<Func<T, bool>>>? filters = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T> query = _entities.AsQueryable();

            foreach(var filter in filters)
            {
                if (filter != null)
                    query = query.Where(filter);
            }
          
            if (includesProperties != null && includesProperties.Any())
            {
                foreach (var include in includesProperties)
                    query = query.Include(include);
            }

            if (orderBy != null)
                query = orderBy(query);
            else
                query = query.OrderBy(e => e.Id);

            return await query.ToListAsync(cancellationToken);
        }
        public async Task<T?> FirstOrDefaultAsync(
         Expression<Func<T, bool>> filter,
         CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _entities.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync(cancellationToken);
        }
        public async Task AddAsync(T entity,
         CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }


        public Task UpdateAsync(T entity,
         CancellationToken cancellationToken = default)
        {
            var existing = _entities.FindAsync(entity.Id, cancellationToken);
            if(existing == null)
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} with id {entity.Id} not found");
            }
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }


        public Task DeleteAsync(T entity,
         CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            return Task.CompletedTask;
            
        }


    }
}
