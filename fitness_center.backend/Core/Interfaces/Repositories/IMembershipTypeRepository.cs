using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IMembershipTypeRepository
    {
        Task<MembershipType?> GetByIdAsync(Guid id, CancellationToken cansel);
        Task<IEnumerable<MembershipType>> GetAllAsync(CancellationToken cansel);
        Task AddAsync(MembershipType type, CancellationToken cansel);
        Task UpdateAsync(MembershipType type, CancellationToken cansel);
        Task DeleteAsync(Guid id, CancellationToken cansel);
    }
}
