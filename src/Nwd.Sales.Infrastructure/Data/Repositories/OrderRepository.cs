using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    public class OrderRepository : CosmosDbRepository<OrderAgg>, IOrderRepository
    {
        private readonly IMapper _mapper;

        public override string ContainerName { get; } = "Orders";

        public override string GenerateId(OrderAgg entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public OrderRepository(IMapper mapper, ICosmosDbContainerFactory factory) : base(factory)
        {
            _mapper = mapper;
        }
    }
}
