using AutoMapper;
using Microsoft.Azure.Cosmos;
using Nwd.Sales.Domain.Orders;
using Nwd.Sales.Infrastructure.Data.Interfaces;

namespace Nwd.Sales.Infrastructure.Data.Repositories
{
    public class OrderRepository : CosmosDbRepository<Order>, IOrderRepository
    {
        private readonly IMapper _mapper;

        public override string ContainerName { get; } = "Orders";

        public override string GenerateId(Order entity) => $"{entity.Id}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public OrderRepository(IMapper mapper, ICosmosDbContainerFactory factory) : base(factory)
        {
            _mapper = mapper;
        }

        public override async Task AddAsync(Order item)
        {
            var id = GenerateId(item);
            await _container.CreateItemAsync<Entities.Order>(_mapper.Map<Entities.Order>(item), ResolvePartitionKey(id));
        }
    }
}
