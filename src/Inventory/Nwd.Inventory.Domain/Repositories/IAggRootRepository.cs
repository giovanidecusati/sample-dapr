using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Domain.Repositories
{
    public interface IAggRootRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(string categoryId, CancellationToken cancellationToken = default);
        Task AddAsync(T item, CancellationToken cancellationToken = default);
    }
}
