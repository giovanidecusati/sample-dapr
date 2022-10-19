namespace Nwd.Sales.Domain.Orders
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid customerId);
    }
}
