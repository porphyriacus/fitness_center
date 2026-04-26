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
        Task<MembershipType?> GetByIdAsync(Guid id);
        Task<IEnumerable<MembershipType>> GetAllAsync();
        Task AddAsync(MembershipType type);
        Task UpdateAsync(MembershipType type);
        Task DeleteAsync(Guid id);
    }
}
