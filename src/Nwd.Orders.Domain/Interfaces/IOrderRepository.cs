using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order entity);
    }
}
