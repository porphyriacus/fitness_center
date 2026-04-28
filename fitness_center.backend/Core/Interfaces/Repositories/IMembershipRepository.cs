using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IMembershipRepository
    {
        Task<Membership?> GetByIdAsync(Guid id, CancellationToken cansel);
        Task<IEnumerable<Membership>> GetByClientIdAsync(Guid clientId, CancellationToken cansel);
        Task AddAsync(Membership membership, CancellationToken cansel);
        Task UpdateAsync(Membership membership, CancellationToken cansel);
        Task DeleteAsync(Guid id, CancellationToken cansel);    
    }
}
