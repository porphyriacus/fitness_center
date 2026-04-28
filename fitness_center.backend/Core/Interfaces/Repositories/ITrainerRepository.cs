using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ITrainerRepository
    {
        Task<Trainer?> GetByIdAsync(Guid id, CancellationToken cancellation);
        Task<IEnumerable<Trainer>> GetAllAsync(CancellationToken cancellation);

        Task AddAsync(Trainer trainer, CancellationToken cancellation);
        Task UpdateAsync(Trainer trainer, CancellationToken cancellation);
        Task DeleteAsync(Guid id, CancellationToken cancellation);
    }
}
