using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(string categoryId, CancellationToken cancellationToken = default);
        Task AddAsync(Category item, CancellationToken cancellationToken = default);
    }
}
