using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Repositories
{
    internal class EfClientRepository : EfRepository<Client>, IClientRepository
    {
        public EfClientRepository(AppDbContext context) : base(context){}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber">номер страницы</param>
        /// <param name="pageSize">сколько записей на странице</param>
        /// <param name="searchTerm">по какому полю производится поис</param>
        /// <param name="sortedBy">по какому полю сортировать</param>
        /// <param name="filters">набор фильтров для WHERE</param>
        /// <param name="includeProperties">параметры INCLUDE</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<(IReadOnlyList<Client>, int TotalCount)> GetPagedAsync(
              int pageNumber
            , int pageSize
            , Func<IQueryable<Client>, IOrderedQueryable<Client>>? sortedBy = default
            , List<Expression<Func<Client, bool>>>? filters = null
            , Expression<Func<Client, object>>[]? includeProperties = default
            , CancellationToken cancellationToken = default)
        {
            IQueryable<Client> query = _entities.AsQueryable();

            if(filters != null) {
                foreach (var filter in filters)
                {
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                }
            }

            int totalCount = await query.CountAsync(cancellationToken); // TotalCount

            if (includeProperties != null && includeProperties.Any())
            {
                foreach(var include in includeProperties)
                    query = query.Include(include);
            }

            if(sortedBy != null)
            {
                query = sortedBy(query);
            }
            else
            {
                query = query.OrderBy(c => c.Id);
            }

            var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

            return (items, totalCount);

        }
    }
}
