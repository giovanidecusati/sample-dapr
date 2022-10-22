using Microsoft.Azure.Cosmos;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Domain.Interfaces;
using Nwd.Orders.Infrastructure.Data.Interfaces;

namespace Nwd.Orders.Infrastructure.Data.Repositories
{
    internal class OrderRepository : CosmosDbRepository<Order>, IOrderRepository
    {
        public override string ContainerName { get; } = CosmosDbContainer.OrdersContainerName;

        public override string GenerateId(Order entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public OrderRepository(ICosmosDbContainerFactory factory) : base(factory)
        {

        }
    }
}
