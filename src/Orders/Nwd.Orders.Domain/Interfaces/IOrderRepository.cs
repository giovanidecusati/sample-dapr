using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order entity, CancellationToken cancellationToken = default);
        Task<Order> GetByIdAsync(string orderId, CancellationToken cancellationToken = default);
    }
}
