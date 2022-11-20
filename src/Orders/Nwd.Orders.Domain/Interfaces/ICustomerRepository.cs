using Nwd.Orders.Domain.Entities;

namespace Nwd.Orders.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(string customerId);
        Task AddAsync(Customer item);
    }
}
