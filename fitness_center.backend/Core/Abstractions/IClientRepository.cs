using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IClientRepository : IRepository<Client>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber">номер страницы</param>
        /// <param name="pageSize">сколько записей на странице</param>
        /// <param name="sortedBy">по какому полю сортировать</param>
        /// <param name="filters">набор фильтров для WHERE</param>
        /// <param name="includeProperties">параметры INCLUDE</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(IReadOnlyList<Client>, int TotalCount)> GetPagedAsync(
            int pageNumber
            , int pageSize
            , Func<IQueryable<Client>, IOrderedQueryable<Client>>? sortedBy = default
            , List<Expression<Func<Client, bool>>>? filters = null
            , Expression<Func<Client, object>>[]? includeProperties = default
            , CancellationToken cancellationToken = default);
    }
}
