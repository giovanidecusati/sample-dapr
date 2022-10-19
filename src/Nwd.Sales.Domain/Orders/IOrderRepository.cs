namespace Nwd.Sales.Domain.Orders
{
    public interface IOrderRepository
    {
        Task SaveAsync(OrderAgg entity);                
    }
}
