using Microsoft.Azure.Cosmos;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;
using Nwd.Orders.Infrastructure.Data.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class CustomerRepository : CosmosDbRepository<Customer>, ICustomerRepository
    {
        public override string ContainerName { get; } = CosmosDbContainer.CustomersContainerName;

        public override string GenerateId(Customer entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public CustomerRepository(ICosmosDbContainerFactory factory) : base(factory)
        {

        }
    }
}
