using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    internal class OrderRepository : CosmosDbRepository<Order, Entities.Order>, IOrderRepository
    {
        public override string ContainerName { get; } = CosmosDbContainer.OrdersContainerName;

        public override string GenerateId(Order entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public OrderRepository(ICosmosDbContainerFactory factory, IMapper mapper) : base(factory, mapper)
        {

        }
    }
}
