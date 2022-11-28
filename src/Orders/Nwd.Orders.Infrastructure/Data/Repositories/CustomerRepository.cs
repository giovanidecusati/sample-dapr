using Dapr.Client;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class CustomerRepository : DaprRepositoryBase<Customer>, ICustomerRepository
    {
        public override string StoreName { get; } = nameof(Customer);

        public override string StoreKey(Customer entity) => $"{Guid.NewGuid()}";

        public CustomerRepository(DaprClient daprClient) : base(daprClient) { }
    }
}
