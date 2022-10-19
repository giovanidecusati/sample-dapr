namespace Nwd.Sales.Domain.Orders
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(string customerId);
        Task AddAsync(Customer item);
    }
}
