using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    internal class CustomerRepository : CosmosDbRepository<Customer, Entities.Customer>, ICustomerRepository
    {
        public override string ContainerName { get; } = CosmosDbContainer.CustomersContainerName;

        public override string GenerateId(Customer entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public CustomerRepository(ICosmosDbContainerFactory factory, IMapper mapper) : base(factory, mapper)
        {

        }
    }
}
