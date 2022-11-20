using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string productId);
        Task AddAsync(Product item);
    }
}
