using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin?> GetByIdAsync(Guid id, CancellationToken cancellation);
        Task<IEnumerable<Admin>> GetAllAsync(CancellationToken cancellation);

        Task AddAsync(Admin admin, CancellationToken cancellation);
        Task UpdateAsync(Admin admin, CancellationToken cancellation);
        Task DeleteAsync(Guid id, CancellationToken cancellation);
    }
}
