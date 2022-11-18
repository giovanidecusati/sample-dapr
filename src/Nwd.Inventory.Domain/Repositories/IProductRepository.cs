using Nwd.Inventory.Domain.Entities;

namespace Nwd.Inventory.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string productId);
        Task AddAsync(Product item);
    }
}
