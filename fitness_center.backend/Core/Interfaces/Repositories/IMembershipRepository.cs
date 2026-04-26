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
        Task<Membership?> GetByIdAsync(Guid id);
        Task<IEnumerable<Membership>> GetByClientIdAsync(Guid clientId);
        Task AddAsync(Membership membership);
        Task UpdateAsync(Membership membership);
        Task DeleteAsync(Membership membership);    
    }
}
