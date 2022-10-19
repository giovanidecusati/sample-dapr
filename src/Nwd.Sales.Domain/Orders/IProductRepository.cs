namespace Nwd.Sales.Domain.Orders
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid productId);
    }
}
