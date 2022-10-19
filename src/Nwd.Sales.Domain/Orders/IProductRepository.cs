namespace Nwd.Sales.Domain.Orders
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(string productId);
        Task AddAsync(Product item);
    }
}
