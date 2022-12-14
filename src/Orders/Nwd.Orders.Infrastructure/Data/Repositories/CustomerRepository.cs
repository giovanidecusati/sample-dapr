using Dapr.Client;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class CustomerRepository : DaprRepositoryBase<Customer>, ICustomerRepository
    {
        public override string StoreName { get; } = "nwd-orders-customer";

        public override string GenerateId(Customer entity) => $"{Guid.NewGuid()}";

        public CustomerRepository(DaprClient daprClient) : base(daprClient) { }
    }
}
