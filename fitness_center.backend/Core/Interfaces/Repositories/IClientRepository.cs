using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellation);
        Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellation);

        Task AddAsync(Client client, CancellationToken cancellation);
        Task UpdateAsync(Client client, CancellationToken cancellation);
        Task DeleteAsync(Guid id, CancellationToken cancellation);
    }
}
