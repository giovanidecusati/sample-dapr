using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string productId, CancellationToken cancellationToken = default);
        Task AddAsync(Product item, CancellationToken cancellationToken = default);
    }
}
