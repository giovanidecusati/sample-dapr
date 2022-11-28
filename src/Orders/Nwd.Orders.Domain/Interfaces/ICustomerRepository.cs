using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(string customerId, CancellationToken cancellationToken = default);
        Task AddAsync(Customer item, CancellationToken cancellationToken = default);
    }
}
